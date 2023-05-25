using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace PokemonApp.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly ISender _sender;

        public ReviewsController(ISender sender)
        { 
            sender = _sender;
        }
    }
}
