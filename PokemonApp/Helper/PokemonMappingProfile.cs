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
            CreateMap<LoginDto, User>();
            CreateMap<UpdateAddressDto, User>().ForMember(r => r.Address, c => c.MapFrom(dto => new Address()
            {
                City = dto.City, Country = dto.Country, PostalCode = dto.PostalCode
            }));
        }
    }
}
