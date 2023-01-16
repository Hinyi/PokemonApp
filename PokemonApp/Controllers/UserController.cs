using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonApp.Data;
using PokemonApp.Interfaces;
using PokemonApp.Models.UserDto;

namespace PokemonApp.Controllers
{
    [Route("api/controller")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public ActionResult RegisterUser([FromBody] RegisterUserDto dto) 
        {
            _userRepository.RegisterUser(dto);
            return Ok();
        }

    }
}
