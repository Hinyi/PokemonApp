namespace PokemonApp.Entities
{
    public class Pokemon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime DateOfBirth { get; set; }

        public List<Review> Reviews { get; set; }
        public List<PokemonUser> PokemonUsers { get; set; }
        //public List<PokemonCategory> PokemonCategories { get; set; }
        public virtual ICollection<Category> Categories { get; set; }

    }
}
