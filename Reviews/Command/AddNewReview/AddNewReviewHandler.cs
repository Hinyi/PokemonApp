using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Reviews.Models;

namespace Reviews.Command.AddNewReview;

public class AddNewReviewHandler : IRequestHandler<AddNewReviewCommand, Unit>
{
    private readonly IMongoCollection<Review> _collection;

    public AddNewReviewHandler(IMongoCollection<Review> collection)
    {
        _collection = collection;
    }
    public async Task<Unit> Handle(AddNewReviewCommand request, CancellationToken cancellationToken)
    {
        var review = new Review()
        {
            Name = request.Name,
            Description = request.Description,
            Rating = request.Rating,
            PokemonId = request.PokemonId,
        };

        await _collection.InsertOneAsync(review);
        return Unit.Value;
        //await Task.FromResult(Unit.Value);
    }
}

