namespace PokemonApp.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }

        //public virtual List<PokemonCategory> PokemonCategories { get; set; }
        public virtual ICollection<Pokemon> Pokemons { get; set; }
    }
}
