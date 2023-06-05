using PokemonApp.Entities;
using PokemonApp.Models.PokemonDto;
using PokemonApp.Repository;

namespace PokemonApp.Interfaces
{
    public interface IPokemonRepository
    {
        bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon);
        bool CreatePokemon(string cat, Pokemon pokemon);
        ICollection<Pokemon> GetPokemons();
        public Pokemon GetPokemonTrimToUpper(PokemonDto pokemonCreate);
        bool Save();
        Task<PagedResult<GetAllPokemonsPaginated>> GetAllPokemonsPaged(GetPokemons query);
    }
}
