using System.ComponentModel;
using System.Reflection;
using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NLog;
using NLog.Web;
using PokemonApp.Data;
using PokemonApp.Entities;
using PokemonApp.Helper;
using PokemonApp.Interfaces;
using PokemonApp.Middleware;
using PokemonApp.Models.UserDto;
using PokemonApp.Repository;
using PokemonApp.Service.CachingService;
using PokemonApp.Service.UserContext;
using Reviews;
using Reviews.Command.AddNewReview;

namespace PokemonApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Logger
            var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
            logger.Debug("init main");

            //JWT authorization
            var authenticationSettings = new AuthenticationSettings();
            builder.Configuration.GetSection("Authentication").Bind(authenticationSettings);
            builder.Services.AddSingleton(authenticationSettings);
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Bearer";
                options.DefaultScheme = "Bearer";
                options.DefaultChallengeScheme = "Bearer";
            }).AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = authenticationSettings.JwtIssuer,
                    ValidAudience = authenticationSettings.JwtIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey)),
                };
            });

            // Add services to the container.
            builder.Services.AddDbContext<PokemonDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("RestaurantDbConnection"));
            });

            //Add redis
            builder.Services.AddStackExchangeRedisCache(redisOption =>
            {
                string connection = builder.Configuration
                    .GetConnectionString("Redis");
                redisOption.Configuration = connection;
            });
            //add mongo db
            ReviewsModule.AddMongoDbCollection(builder.Services,builder.Configuration);
            //add memory cache
            builder.Services.AddMemoryCache();
            //Add redis
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSingleton<ICacheService, CacheService>();
            //Added services of sedder
            builder.Services.AddScoped<Seeder>();
            //Hash password service
            builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            //Added automapper
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            //Adding middleware
            builder.Services.AddScoped<ErrorHandlingMiddleware>();
            //Services
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<PokemonRepository>();
            builder.Services.AddScoped<IPokemonRepository, CachedPokemonRepository>();

            builder.Services.AddControllers().AddFluentValidation();
            //Validation
            builder.Services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
            //usercontext service
            builder.Services.AddScoped<IUserContext, UserContextService>();
            builder.Services.AddHttpContextAccessor();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //Nlogger
            builder.Logging.ClearProviders();
            builder.Host.UseNLog();


            //mediatr
            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(AddNewReviewHandler).Assembly);
            });
            //builder.Services.AddMediatR(typeof(Program).Assembly);
            var app = builder.Build();

            //Seed database if is null
            SeedDatabase();

            //Use error handling middleware
            app.UseMiddleware<ErrorHandlingMiddleware>();
            //adding jwt response in header
            //app.UseSession();
            //app.Use(async (context, next) =>
            //{
            //    var token = context.Session.GetString("Token");
            //    if (!string.IsNullOrEmpty(token))
            //    {
            //        context.Request.Headers.Add("Authorization", "Bearer " + token);
            //    }
            //    await next();
            //});

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                //app.UseSwagger();
                //app.UseSwaggerUI();
            }

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseAuthentication();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();


            void SeedDatabase() //can be placed at the very bottom under app.Run()
            {
                using (var scope = app.Services.CreateScope())
                {
                    var dbInitializer = scope.ServiceProvider.GetRequiredService<Seeder>();
                    dbInitializer.Seed();
                }
            }
        }
    }
}