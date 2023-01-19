using Microsoft.EntityFrameworkCore;
using PokemonApp.Data;
using PokemonApp.Entities;

namespace PokemonApp
{
    public class Seeder
    {
        private readonly PokemonDbContext _dbContext;

        public Seeder(PokemonDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Seed()
        {
            if (_dbContext.Database.CanConnect())
            {
                if (!_dbContext.Roles.Any())
                {
                    var roles = getRoles();
                    _dbContext.Roles.AddRange(roles);
                    _dbContext.SaveChanges();
                }    
                if (!_dbContext.Gyms.Any())
                {
                    var gyms = getGyms();
                    _dbContext.Gyms.AddRange(gyms);
                    _dbContext.SaveChanges();
                }
            }
        }

        private IEnumerable<Role> getRoles()
        {
            var roles = new List<Role>()
            {
                new Role()
                {
                    Name = "User"
                },
                new Role()
                {
                    Name = "Manager"
                },
                new Role()
                {
                    Name = "Admin"
                }
            };
            return roles;
        }
        private IEnumerable<Gym> getGyms()
        {
            var gyms = new List<Gym>()
            {
                new Gym()
                {
                    Name = "Pewter Gym"
                },
                new Gym()
                {
                    Name = "Cerulean Gym"
                },
                new Gym()
                {
                    Name = "Vermilion Gym"
                },
                new Gym()
                {
                    Name = "string"
                }
            };
            return gyms;
        }

    }
}
