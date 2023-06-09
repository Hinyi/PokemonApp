using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using MongoDB.Driver;
using PokemonApp.Service.CachingService;
using Reviews.Models;

namespace Reviews.Query.GetReviewsByName
{
    public class GetReviewsByNameQuery : IRequest<List<Review>>
    {
        public string Name { get; set; }
    }
}
