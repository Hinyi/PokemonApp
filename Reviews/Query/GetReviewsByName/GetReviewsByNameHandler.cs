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
    public class GetReviewsByNameHandler : IRequestHandler<GetReviewsByNameQuery, List<Review>>
    {

        private readonly IMongoCollection<Review> _collection;
        private readonly ICacheService _cacheService;

        public GetReviewsByNameHandler(IMongoCollection<Review> collection, ICacheService cacheService)
        {
            _collection = collection;
            _cacheService = cacheService;
        }
        public async Task<List<Review>> Handle(GetReviewsByNameQuery request, CancellationToken cancellationToken)
        {
            return await _cacheService.GetAsync(
                $"{request.Name}-review",
                async () =>
                {
                    IReadOnlyList<Review> reviews = await _collection
                        .Find(x => x.Name == request.Name).ToListAsync();

                    return reviews.ToList();
                });
        }
    }
}
