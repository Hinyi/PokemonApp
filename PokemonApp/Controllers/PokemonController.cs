using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonApp.Data;
using PokemonApp.Entities;
using PokemonApp.Exceptions;
using PokemonApp.Interfaces;
using PokemonApp.Models.PokemonDto;
using PokemonApp.Repository;
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

        [HttpGet("getallpaged")]
        public async Task<ActionResult> GetAllPokemons([FromQuery] GetPokemons query)
        {
            var pokemons = await _pokemonRepository.GetAllPokemonsPaged(query);
            //var result = _mapper.Map<PokemonDto>(pokemons);
            return Ok(pokemons);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(PokemonDto))]
        [ProducesResponseType(400)]
        public async Task<ActionResult> GetPokemonById(int id)
        {
            var pokemon = await _pokemonRepository.GetPokemon(id);
            if (pokemon is null)
            {
                //return NotFound("Pokemon not found");
                throw new NotFoundException("Pokemon not found");
            }
            var result = _mapper.Map<GetPokemonDto>(pokemon);
            return Ok(result);
        }

        [HttpGet("getpokemonbyname")]
        public async Task<ActionResult> GetPokemonByName(string name)
        {
            var pokemon = await _pokemonRepository.GetPokemon(name);
            //var result = _mapper.Map<GetPokemonDto>(pokemon);
            return Ok(pokemon);
        }

        [HttpPost("createpokemon")]
        public IActionResult CreatePokemon([FromQuery] int userId, [FromQuery] int catId,
            [FromBody] PokemonDto pokemonCreate)
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

            bool userIsLoggedIn = _userContext.User is null ? false : true;

            if (userIsLoggedIn)
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