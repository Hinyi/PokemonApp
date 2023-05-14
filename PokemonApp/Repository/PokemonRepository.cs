using Microsoft.EntityFrameworkCore.Diagnostics;
using PokemonApp.Data;
using PokemonApp.Entities;
using PokemonApp.Interfaces;
using PokemonApp.Models.PokemonDto;

namespace PokemonApp.Repository
{
    public class PokemonRepository : IPokemonRepository
    {
        private readonly PokemonDbContext _context;

        public PokemonRepository(PokemonDbContext context)
        {
            _context = context;
        }

        public bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon)
        {
            var pokemonOwnerEntity = _context.Users.FirstOrDefault(a => a.Id == ownerId);
            var category = _context.Categories.FirstOrDefault(x => x.Id == categoryId);


            var newPokemon = new Pokemon()
            {
                Name = pokemon.Name,
                DateOfBirth = DateTime.Now,
            };

            var pokemonCategory = new PokemonCategory()
            {
                Category = category,
                Pokemon = newPokemon,
            };

            var pokemonOwner = new PokemonUser()
            {
                User = pokemonOwnerEntity,
                Pokemon = newPokemon,

            };

            _context.Add(pokemonCategory);
            _context.Add(pokemonOwner);
            _context.Add(newPokemon);

            return Save();
        }

        public bool CreatePokemon(int categoryId, Pokemon pokemon)
        {
            throw new NotImplementedException();
        }

        public ICollection<Pokemon> GetPokemons()
        {
            return _context.Pokemons.OrderBy(x => x.Id).ToList();
        }

        public Pokemon GetPokemonTrimToUpper(PokemonDto pokemonCreate)
        {
            return GetPokemons().Where(c => c.Name.Trim().ToUpper() == pokemonCreate.Name.TrimEnd().ToUpper())
                .FirstOrDefault();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
