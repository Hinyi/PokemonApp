using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using Reviews.Models;

namespace Reviews.Query.GetAllReviews
{
    public class GetAllReviewsHandler : IRequestHandler
        <GetAllReviewsQuery, List<Review>>
    {
        private readonly IMongoCollection<Review> _collection;

        public GetAllReviewsHandler(IMongoCollection<Review> collection)
        {
            _collection = collection;
        }
        public async Task<List<Review>> Handle(GetAllReviewsQuery request, CancellationToken cancellationToken)
        {
            var result = await _collection.Find(_ => true).ToListAsync();

            //return result is null ? null : result;
            return await Task.FromResult(result);
        }
    }
}
