using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonApp.Data;
using PokemonApp.Interfaces;
using PokemonApp.Models.UserDto;

namespace PokemonApp.Controllers
{
    [Route("api/[controller]")]
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
        
        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginDto dto)
        {
            string token = _userRepository.GenerateJwt(dto);
            return Ok(token);
        }

        [HttpPut("{id}/address")]
        public ActionResult UpdateAddress([FromRoute] int id, [FromBody] UpdateAddressDto dto)
        {
            _userRepository.UpdateAddress(id, dto);
            return Ok();
        }        
        [HttpPut("{id}/gym")]
        public async Task<ActionResult> UpdateGym([FromRoute] int id, [FromBody] UpdateUserGymDto dto)
        {
            await _userRepository.UpdateGym(id, dto);
            return Ok();
        }

        [HttpDelete("delete")]
        public ActionResult Delete()
        {
            _userRepository.DeleteUser();

            return NoContent();
        }

    }
}
