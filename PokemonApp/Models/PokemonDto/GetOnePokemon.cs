using Microsoft.EntityFrameworkCore;

namespace PokemonApp.Models.PokemonDto
{
    public class GetOnePokemon<T>
    {
        public List<T> Pokemon { get; set; }

        public GetOnePokemon(List<T> pokemon)
        {
            Pokemon = pokemon;
        }

        public static async Task<GetOnePokemon<T>> CreateAsync(IQueryable<T> pokemon)
        {
            var item = await pokemon.ToListAsync();
            return new GetOnePokemon<T>(item);
        }
    }
}
