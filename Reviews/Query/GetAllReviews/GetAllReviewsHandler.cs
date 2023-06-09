using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.Runtime.Internal.Util;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using MongoDB.Driver;
using Reviews.Models;
using PokemonApp.Service.CachingService;

namespace Reviews.Query.GetAllReviews
{
    public class GetAllReviewsHandler : IRequestHandler
        <GetAllReviewsQuery, List<Review>>
    {
        private readonly IMongoCollection<Review> _collection;
        private readonly ICacheService _cacheService;

        public GetAllReviewsHandler(IMongoCollection<Review> collection, ICacheService cacheService)
        {
            _collection = collection;
            _cacheService = cacheService;
        }
        public async Task<List<Review>> Handle(GetAllReviewsQuery request, CancellationToken cancellationToken)
        {
            List<Review> reviewResponse = await _cacheService
                .GetAsync<List<Review>>("review");

            if (reviewResponse is not null)
            {
                return reviewResponse;
            }

            var result = await _collection.Find(_ => true).ToListAsync();

            reviewResponse = result.ToList();
            await _cacheService.SetAsync("review", reviewResponse);

            //return result is null ? null : result;
            //return await Task.FromResult(result);
            return reviewResponse;
        }
    }
}
