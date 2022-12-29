namespace PokemonApp.Entities
{
    public class Reviewer
    {
        public int Id { get; set; }
        public string Nickname { get; set; }

        public virtual List<Review> Reviews { get; set; }
    }
}
