﻿using AutoMapper;
using PokemonApp.Entities;
using PokemonApp.Models.PokemonDto;
using PokemonApp.Models.ReviewDto;
using PokemonApp.Models.UserDto;
using Review = Reviews.Models.Review;

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
            CreateMap<UpdateUserGymDto, User>();
            CreateMap<PokemonDto, Pokemon>();
            CreateMap<Pokemon, PokemonDto>();    
            
            CreateMap<GetPokemonDto, Pokemon>();
            CreateMap<Pokemon, GetPokemonDto>();

            CreateMap<AddNewReviewDto, Review>();
            CreateMap<Review, AddNewReviewDto>();
        }
    }
}
