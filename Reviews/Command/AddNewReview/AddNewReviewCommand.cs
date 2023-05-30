using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Reviews.Models;
using Reviews.ReadModels;
using ZstdSharp.Unsafe;

namespace Reviews.Command.AddNewReview
{
	// public record AddNewReviewCommand(int PokemondId, ReviewReadModel reviewReadModel) : IRequest<Unit>

	public class AddNewReviewCommand : IRequest<Unit> 
	{
        public string Name { get; set; }
        public string Description { get; set; }

        public decimal Rating { get; set; }
        public int PokemonId { get; set; }

        //public AddNewReviewCommand()
        //{
        //    RuleFor(x => x.Name).NotEmpty().MinimumLength(3).MaximumLength(16);
        //    RuleFor(x => x.Description).NotEmpty().MinimumLength(1).MaximumLength(160);
        //    //RuleFor(x => x.Rating).GreaterThanOrEqualTo(0).LessThanOrEqualTo(10).WithMessage("Rating cannot be greater than 10 and less than 0");
        //    RuleFor(x => x.PokemonId).NotEmpty();
        //}
    }
}
