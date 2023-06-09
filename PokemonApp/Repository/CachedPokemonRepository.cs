using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using PokemonApp.Entities;
using PokemonApp.Helper;
using PokemonApp.Interfaces;
using PokemonApp.Models.PokemonDto;

namespace PokemonApp.Repository
{
    public class CachedPokemonRepository : IPokemonRepository
    {
        private readonly PokemonRepository _decorated;
        private readonly IMemoryCache _memoryCache;
        private readonly IDistributedCache _distributedCache;

        public CachedPokemonRepository(PokemonRepository decorated, IMemoryCache memoryCache, IDistributedCache distributedCache)
        {
            _decorated = decorated;
            _memoryCache = memoryCache;
            _distributedCache = distributedCache;
        }
        public ICollection<Pokemon> GetPokemons() =>
            _decorated.GetPokemons();

        public async Task<Pokemon> GetPokemon(int id)
        {
            string key = $"member-{id}";

            string? cachedPokemon = await _distributedCache.GetStringAsync(key);

            Pokemon? pokemon;
            if (string.IsNullOrEmpty(cachedPokemon))
            {
                pokemon = await _decorated.GetPokemon(id);

                if (pokemon is null)
                    return pokemon;

                await _distributedCache.SetStringAsync(
                    key,
                    JsonConvert.SerializeObject(pokemon));
                return pokemon;
            }

            pokemon = JsonConvert.DeserializeObject<Pokemon>(cachedPokemon,
                new JsonSerializerSettings
                {
                    //if constructor is not public
                    ConstructorHandling = 
                        ConstructorHandling.AllowNonPublicDefaultConstructor,
                    //if set property is private
                    ContractResolver = new PrivateResolver()
                });

            return pokemon;
        }

        public async Task<GetOnePokemon<GetPokemonDto>> GetPokemon(string name)
        {
            string key = $"member-{name}";
            return await _memoryCache.GetOrCreateAsync(
                key,
                entry =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));

                    return _decorated.GetPokemon(name);
                });
        }

        public bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon) =>
            _decorated.CreatePokemon(ownerId, categoryId, pokemon);

        public bool CreatePokemon(string cat, Pokemon pokemon) =>
            _decorated.CreatePokemon(cat, pokemon);


        public Pokemon GetPokemonTrimToUpper(PokemonDto pokemonCreate) =>
            _decorated.GetPokemonTrimToUpper(pokemonCreate);

        public bool Save() =>
            _decorated.Save();

        public Task<PagedResult<GetAllPokemonsPaginated>> GetAllPokemonsPaged(GetPokemons query) =>
            _decorated.GetAllPokemonsPaged(query);

    }
}