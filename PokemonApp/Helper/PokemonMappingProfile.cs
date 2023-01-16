using AutoMapper;
using PokemonApp.Entities;
using PokemonApp.Models.UserDto;

namespace PokemonApp.Helper
{
    public class PokemonMappingProfile : Profile
    {
        public PokemonMappingProfile()
        {
            CreateMap<RegisterUserDto, User>();

        }
    }
}
