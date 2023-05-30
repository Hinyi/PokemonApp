using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using Reviews.DataAccess;
using Reviews.Exceptions;
using Reviews.Models;
using Shared.Common;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace Reviews;

public class ReviewsModule
{
    public static void AddMongoDbCollection(IServiceCollection services, IConfiguration configuration)
    {
        //string fileName = "D:\\Smieci\\ProjektyCsharp\\PokemonApp\\Reviews\\mongosettings.json";
        //string mongosettings = File.ReadAllText(fileName);
        //MongoDbSettings? settings = JsonSerializer.Deserialize<MongoDbSettings>(mongosettings);

        var settings = new MongoDbSettings()
        {
            ConnectionString = "mongodb://localhost:27017",
            DatabaseName = "Reviews",
            ReviewsCollectionName = "Reviews"
        };

        var mongoClient = new MongoClient(settings.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(settings.DatabaseName);

        if (!isMongoLive(mongoDatabase))
        {
            throw new DatabaseNotConnected();
        }

        services.AddScoped<IMongoCollection<Review>>(_ =>
            mongoDatabase.GetCollection<Review>(settings.ReviewsCollectionName));
    }

    static bool isMongoLive(IMongoDatabase mongoDatabase)
    {
        return mongoDatabase.RunCommandAsync((Command<BsonDocument>)"{ping:1}").Wait(1000);
    }
}