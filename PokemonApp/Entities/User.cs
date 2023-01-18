namespace PokemonApp.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime? CreatedTime { get; set; }

        public int RoleId { get; set; }
        public virtual Role Role { get; set; }

        public int? AddressId { get; set; }
        public virtual Address? Address { get; set; }
        
        public int? GymId { get; set; }
        public virtual Gym? Gym { get; set; }

        public virtual List<PokemonUser>? PokemonUsers { get; set; }

    }
}
