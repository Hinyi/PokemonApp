using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PokemonApp.Controllers;
using PokemonApp.Entities;
using PokemonApp.Interfaces;
using PokemonApp.Models.PokemonDto;
using PokemonApp.Service.UserContext;
using Xunit;

namespace PokemonAppTests
{
    public class PokemonControllerTests
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;
        private HttpClient _client;

        public PokemonControllerTests()
        {
            _pokemonRepository = A.Fake<IPokemonRepository>();
            _mapper = A.Fake<IMapper>();
            _userContext = A.Fake<IUserContext>();
        }

        [Fact]
        public void PokemonController_GetPokemons_ReturnOK()
        {
            //arange
            var pokemons = A.Fake<ICollection<PokemonDto>>();
            var pokemonsList = A.Fake<List<PokemonDto>>();
            A.CallTo(() => _mapper.Map<List<PokemonDto>>(pokemons)).Returns(pokemonsList);
            var controller = new PokemonController(_pokemonRepository, _mapper, _userContext);

            //act
            var result = controller.GetPokemons();
            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
        }

        //[Fact]
        //public void PokemonController_GetPokemons_InvalidModel()
        //{
        //    var pokemons = A.Fake<ICollection<PokemonDto>>();
        //    PokemonDto returnValue = new();
        //    A.CallTo(() => _mapper.Map<List<PokemonDto>>(pokemons)).Returns(returnValue);
        //    var controller = new PokemonController(_pokemonRepository, _mapper, _userContext);

        //    //act
        //    var result = controller.GetPokemons();

        //    result.StatusCode.Should().NotBeNull();
        //}

        [Fact]
        public void PokemonController_CreatePokemon_InvalidModel()
        {
            //arange
            int userId = 1;
            int catId = 1;
            var pokemonCreate = A.Fake<PokemonDto>();
            var pokemon = A.Fake<Pokemon>();
            var pokemonMap = A.Fake<Pokemon>();

            A.CallTo(() => _pokemonRepository.GetPokemonTrimToUpper(pokemonCreate)).Returns(pokemon);
            A.CallTo(() => _mapper.Map<Pokemon>(pokemonCreate)).Returns(pokemon);
            A.CallTo(() => _pokemonRepository.CreatePokemon(userId, catId, pokemonMap)).Returns(true);
            var controller = new PokemonController(_pokemonRepository, _mapper, _userContext);
            //act
            var result = controller.CreatePokemon(userId, catId, pokemonCreate);
            //assert

            result.Should().NotBeNull();
        }
    }
}