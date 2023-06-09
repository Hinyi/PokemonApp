using MediatR;
using Microsoft.AspNetCore.Mvc;
using PokemonApp.Models.ReviewDto;
using Reviews.Command.AddNewReview;
using Reviews.Command.DeleteAllData;
using Reviews.Models;
using Reviews.Query.GetAllReviews;
using Reviews.Query.GetReviewsByName;
using Reviews.ReadModels;

namespace PokemonApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ISender _sender;

        public ReviewsController(ISender sender, IMediator mediator)
        {
            _mediator = mediator;
            _sender = sender;
        }

        [HttpGet("getreviews")]
        public async Task<ActionResult<List<Review>>> GetAllReviews()
        {
            var request = new GetAllReviewsQuery();
            var reviews = await _mediator.Send(request);
            return Ok(reviews);
        }        
        
        [HttpGet("{name}")]
        public async Task<ActionResult<List<Review>>> GetAllReviews(string name)
        {
            var request = new GetReviewsByNameQuery()
            {
                Name = name
            };
            var reviews = await _mediator.Send(request);
            return Ok(reviews);
        }

        [HttpPost("createreview")]
        public async Task<ActionResult> AddNewReview([FromQuery] int pokemonId,[FromBody] ReviewReadModel reviewReadModel)
        {
            var model = new AddNewReviewCommand()
            {
                Name = reviewReadModel.Name,
                Description = reviewReadModel.Description,
                Rating = reviewReadModel.Rating,
                PokemonId = pokemonId
            };

            await _mediator.Send(model);
            return Ok();
        }

        [HttpDelete("deletealldocumentswithinreviews")]
        public async Task<IActionResult> DeleteAllData()
        {
            await _mediator.Send(new DeleteAllDataCommand());
            return Ok();
        }
    }
}
