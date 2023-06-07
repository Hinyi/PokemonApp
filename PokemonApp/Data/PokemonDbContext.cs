using Microsoft.EntityFrameworkCore;
using PokemonApp.Entities;

namespace PokemonApp.Data
{
    public class PokemonDbContext : DbContext
    {
        public PokemonDbContext(DbContextOptions<PokemonDbContext> options) : base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Reviewer> Reviewers { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<PokemonUser> PokemonUsers { get; set; }
        public DbSet<Pokemon> Pokemons { get; set; }
        public DbSet<Gym> Gyms { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Address> Addresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PokemonUser>()
                .HasKey(po => new { po.PokemonId, UserId = po.UserId });
            modelBuilder.Entity<PokemonUser>()
                .HasOne(p => p.Pokemon)
                .WithMany(pc => pc.PokemonUsers)
                .HasForeignKey(p => p.PokemonId);
            modelBuilder.Entity<PokemonUser>()
                .HasOne(p => p.User)
                .WithMany(pc => pc.PokemonUsers)
                .HasForeignKey(c => c.UserId);
        }
    }
}
