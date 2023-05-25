using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonApp.Data;
using PokemonApp.Entities;
using PokemonApp.Interfaces;
using PokemonApp.Models.PokemonDto;
using PokemonApp.Service.UserContext;

namespace PokemonApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : Controller
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;

        public PokemonController(IPokemonRepository pokemonRepository, IMapper mapper, IUserContext userContext)
        {
            _pokemonRepository = pokemonRepository;
            _mapper = mapper;
            _userContext = userContext;
        }

        [HttpGet]
        public IActionResult GetPokemons()
        {
            var pokemons = _mapper.Map<List<PokemonDto>>(_pokemonRepository.GetPokemons());
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(pokemons);
        }

        [HttpPost("createpokemon")]
        public IActionResult CreatePokemon([FromQuery] int userId, [FromQuery] int catId, [FromBody] PokemonDto pokemonCreate)
        {
            if (pokemonCreate == null)
                return BadRequest(ModelState);

            var pokemons = _pokemonRepository.GetPokemonTrimToUpper(pokemonCreate);

            if (pokemons != null)
            {
                ModelState.AddModelError("", "Pokemon already exists");
                return StatusCode(422, ModelState);
            }

            var pokemonMap = _mapper.Map<Pokemon>(pokemonCreate);


            if (!_pokemonRepository.CreatePokemon(userId, catId, pokemonMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }        

        [HttpPost("createpokemonJWT")]
        public IActionResult CreatePokemonJWT([FromBody] PokemonDto pokemonCreate)
        {
            if (pokemonCreate == null)
                return BadRequest(ModelState);

            var pokemons = _pokemonRepository.GetPokemonTrimToUpper(pokemonCreate);

            if (pokemons != null)
            {
                ModelState.AddModelError("", "Pokemon already exists");
                return StatusCode(422, ModelState);
            }

            var pokemonMap = _mapper.Map<Pokemon>(pokemonCreate);

            bool userIsLogged = _userContext.User is null ? false : true;

            if (userIsLogged)
            {
                if (!_pokemonRepository.CreatePokemon(pokemonCreate.Category, pokemonMap))
                {
                    ModelState.AddModelError("", "Something went wrong while saving");
                    return StatusCode(500, ModelState);
                }
            }

            return Ok("Successfully created");
        }
    }
}
