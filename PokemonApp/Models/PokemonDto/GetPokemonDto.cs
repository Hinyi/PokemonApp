namespace PokemonApp.Models.PokemonDto
{
    public class GetPokemonDto
    {
        //public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDateTime { get; set; }
        public IEnumerable<object> Category { get; set; }
    }
}
