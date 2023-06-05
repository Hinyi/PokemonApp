using PokemonApp.Entities;

namespace PokemonApp.Models.PokemonDto
{
    public class GetAllPokemonsPaginated
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<object> Categories { get; set; }
    }
}
