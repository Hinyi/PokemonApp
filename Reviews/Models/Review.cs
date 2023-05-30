using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Reviews.Models
{
    public class Review 
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Rating { get; set; }
        public int PokemonId { get; set; }
        public DateTime CreatedTime = DateTime.Now;

    }
}
