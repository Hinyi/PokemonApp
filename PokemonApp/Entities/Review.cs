namespace PokemonApp.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? UpdatedTime { get; set;}

        public int ReviewerId { get; set; }
        public virtual Reviewer Reviewers { get; set; }
        public int PokemonId { get; set; }
        public virtual Pokemon Pokemons { get; set; }

    }
}
