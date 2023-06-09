using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PokemonApp.Data;
using PokemonApp.Entities;
using PokemonApp.Repository;
using PokemonApp.Service.UserContext;
using Xunit;

namespace PokemonAppTests
{
    public class PokemonRepositoryTests
    {
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;
        private async Task<PokemonDbContext> GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<PokemonDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var databaseContext = new PokemonDbContext(options);
            databaseContext.Database.EnsureCreated();
            if (await databaseContext.Pokemons.CountAsync() <= 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    databaseContext.Pokemons.Add( new Pokemon()
                    {
                        Name = "Pikachu",
                        Description = "ss",
                        DateOfBirth = new DateTime(1998,1,1),
                        Categories = new Collection<Category>()
                        {
                            new Category(){Name = "Electric"},
                            new Category(){Name = "Fire"}
                        }
                    });
            await databaseContext.SaveChangesAsync();
                }
            }
            return databaseContext;
        }

        [Fact]
        public async void PokemonRepository_GetPokemon_ReturnsPokemon()
        {
            //Arrange
            var name = "Pikachu";
            var context = await GetDatabaseContext();
            var pokemonRepository = new PokemonRepository(context, _userContext, _mapper);
            //Act
            var result = pokemonRepository.GetPokemon(name);
            //Assert
            result.Should().NotBeNull();
            result.Result.Name.Should().Be(name);
            result.Should().BeOfType<Pokemon>();
            
        }

        [Fact]
        public async void PokemonRepository_GetPokemonById_ReturnsPokemon()
        {
            //Arrange
            var id = 6;
            var context = await GetDatabaseContext();
            var pokemonRepository = new PokemonRepository(context, _userContext, _mapper);
            //Act
            var result = pokemonRepository.GetPokemon(id);
            //Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(id);
            result.Should().BeOfType<Pokemon>();
        }
    }
}
