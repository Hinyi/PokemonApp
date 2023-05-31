using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PokemonApp.Data;
using PokemonApp.Entities;
using PokemonApp.Exceptions;
using PokemonApp.Helper;
using PokemonApp.Interfaces;
using PokemonApp.Models.UserDto;
using PokemonApp.Service.UserContext;

namespace PokemonApp.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly PokemonDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AuthenticationSettings _authenticationSettings;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContextService;

        public UserRepository(PokemonDbContext context, IPasswordHasher<User> passwordHasher
                ,AuthenticationSettings authenticationSettings, IMapper mapper, IUserContext userContextService)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
            _mapper = mapper;
            _userContextService = userContextService;
        }
        public void RegisterUser(RegisterUserDto dto)
        {

            //var newUser = new User()
            //{
            //    Name = dto.Name,
            //    LastName= dto.LastName,
            //    Nickname= dto.Nickname,
            //    Email = dto.Email,
            //    RoleId= dto.RoleId,
            //    Address = new Address(),
            //    CreatedTime = DateTime.Now,
            //    Gym = new Gym()
            //    {
            //        Name = dto.GymName
            //    },
            //};

            //var hashedPassword = _passwordHasher.HashPassword(newUser, dto.Password);
            //newUser.PasswordHash = hashedPassword;

            var newUser = _mapper.Map<User>(dto);

            var hashedPassword = _passwordHasher.HashPassword(newUser, dto.Password);
            newUser.PasswordHash = hashedPassword;

            _context.Users.Add(newUser);
            _context.SaveChanges();
        }

        public string GenerateJwt(LoginDto dto)
        {
            var user = _context.Users
                .Include(r => r.Role)
                .FirstOrDefault(x => x.Nickname == dto.Nickname);

            if (user == null)
            {
                throw new BadRequestException("Invalid username or password");
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Invalid username or password");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: cred);

            var tokenHandler = new JwtSecurityTokenHandler();
            
            return tokenHandler.WriteToken(token);
        }

        public void UpdateAddress(int id, UpdateAddressDto dto)
        {
            var userAddress = _context.Users.Include(r => r.Address).FirstOrDefault(x => x.Id == id);

            if (userAddress == null)
            {
                throw new NotFoundException("User not found");
            }

            var address = _context.Addresses.FirstOrDefault(x => x.Id == userAddress.AddressId);

            address.City = dto.City;
            address.Country = dto.Country;
            address.PostalCode = dto.PostalCode;

            _context.SaveChanges();
        }       

        public async Task UpdateGym(int id, UpdateUserGymDto dto)
        {
            var userGym = _context.Users.Include(r => r.Gym).FirstOrDefault(x => x.Id == id);

            if (userGym == null)
            {
                throw new NotFoundException("User not found");
            }

            var gymId = _context.Gyms.FirstOrDefault(x => x.Name == dto.Name);
            if (gymId is null)
            {
                throw new NotFoundException("Gym not found");
            }

            userGym.GymId = gymId.Id;

            //_context.Update(gym);
            await _context.SaveChangesAsync();
        }

        public void DeleteUser()
        {
            if (_userContextService.GetUserId is null)
            {
                throw new NotFoundException("User is not logged");
            }

            var user = _context.Users.FirstOrDefault(x =>x.Id == _userContextService.GetUserId);

            if (user is null)
            {
                throw new NotFoundException("User not found");
            }

            _context.Users.Remove(user);
            _context.SaveChanges();
        }
    }
}
