using Microsoft.Extensions.Caching.Memory;
using PokemonApp.Entities;
using PokemonApp.Interfaces;
using PokemonApp.Models.PokemonDto;

namespace PokemonApp.Repository
{
    public class CachedPokemonRepository : IPokemonRepository
    {
        private readonly PokemonRepository _decorated;
        private readonly IMemoryCache _memoryCache;

        public CachedPokemonRepository(PokemonRepository decorated, IMemoryCache memoryCache)
        {
            _decorated = decorated;
            _memoryCache = memoryCache;
        }

        public bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon) =>
            _decorated.CreatePokemon(ownerId, categoryId, pokemon);

        public bool CreatePokemon(string cat, Pokemon pokemon) =>
            _decorated.CreatePokemon(cat, pokemon);

        public ICollection<Pokemon> GetPokemons() =>
            _decorated.GetPokemons();

        public Pokemon GetPokemonTrimToUpper(PokemonDto pokemonCreate) =>
            _decorated.GetPokemonTrimToUpper(pokemonCreate);

        public bool Save() =>
            _decorated.Save();

        public Task<PagedResult<GetAllPokemonsPaginated>> GetAllPokemonsPaged(GetPokemons query) =>
            _decorated.GetAllPokemonsPaged(query);

        public Pokemon GetPokemon(int id)
        {
            string key = $"member-{id}";
            return _memoryCache.GetOrCreate(
                key,
                entry =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));

                    return _decorated.GetPokemon(id);
                });
        }

        public Pokemon GetPokemon(string name)
        {
            string key = $"member-{name}";
            return _memoryCache.GetOrCreate(
                key,
                entry =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));

                    return _decorated.GetPokemon(name);
                });
        }
    }
}