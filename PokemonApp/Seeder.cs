using Microsoft.EntityFrameworkCore;
using PokemonApp.Data;
using PokemonApp.Entities;
using System.Diagnostics.Metrics;

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
                /*
                if (!_dbContext.PokemonUsers.Any())
                {
                    var pokemonOwners = new List<PokemonUser>()
                {
                    new PokemonUser()
                    {
                        Pokemon = new Pokemon()
                        {
                            Name = "Pikachu",
                            DateOfBirth = new DateTime(1903,1,1),
                            PokemonCategories = new List<PokemonCategory>()
                            {
                                new PokemonCategory { Category = new Category() { Name = "Electric", Description = "yes"}}
                            },
                            Reviews = new List<Review>()
                            {
                                new Review { Text="Pikachu",Description = "Pickahu is the best pokemon, because it is electric", Rating = 5,
                                Reviewers = new Reviewer(){Nickname = "Teddy"} },
                                new Review { Text="Pikachu", Description = "Pickachu is the best a killing rocks", Rating = 5,
                                Reviewers = new Reviewer(){Nickname = "Taylor"} },
                                new Review { Text="Pikachu",Description = "Pickchu, pickachu, pikachu", Rating = 1,
                                Reviewers = new Reviewer(){Nickname = "Jessica"} },
                            }
                        },
                        User = new User()
                        {Nickname = "Jack", LastName = "London"}
                    },
                    new PokemonUser()
                    {
                        Pokemon = new Pokemon()
                        {
                            Name = "Squirtle",
                            DateOfBirth = new DateTime(1903,1,1),
                            PokemonCategories = new List<PokemonCategory>()
                            {
                                new PokemonCategory { Category = new Category() { Name = "Water", Description = "yes"}}
                            },
                            Reviews = new List<Review>()
                            {
                                new Review { Text= "Squirtle", Description = "squirtle is the best pokemon, because it is electric", Rating = 5,
                                Reviewers = new Reviewer(){ Nickname = "Teddy"} },
                                new Review { Text= "Squirtle",Description = "Squirtle is the best a killing rocks", Rating = 5,
                                Reviewers = new Reviewer(){ Nickname = "Taylor", } },
                                new Review { Text= "Squirtle", Description = "squirtle, squirtle, squirtle", Rating = 1,
                                Reviewers = new Reviewer(){ Nickname = "Jessica" } },
                            }
                        },
                        User = new User()
                        {
                            Name = "Harry",
                            LastName = "Potter",
                        }
                    },
                                    new PokemonUser()
                    {
                        Pokemon = new Pokemon()
                        {
                            Name = "Venasuar",
                            DateOfBirth = new DateTime(1903,1,1),
                            PokemonCategories = new List<PokemonCategory>()
                            {
                                new PokemonCategory { Category = new Category() { Name = "Leaf", Description = "yes"}}
                            },
                            Reviews = new List<Review>()
                            {
                                new Review { Text= "Veasaur",Description = "Venasuar is the best pokemon, because it is electric", Rating = 5,
                                Reviewers = new Reviewer(){Nickname = "Teddy"} },
                                new Review { Text="Veasaur",Description = "Venasuar is the best a killing rocks", Rating = 5,
                                Reviewers = new Reviewer(){Nickname = "Taylor"} },
                                new Review { Text="Veasaur",Description = "Venasuar, Venasuar, Venasuar", Rating = 1,
                                Reviewers = new Reviewer(){Nickname = "Jessica"} },
                            }
                        },
                        User = new User()
                        {
                            Nickname = "Ash",
                            LastName = "Ketchum",
                        }
                    }
                };
                    _dbContext.PokemonUsers.AddRange(pokemonOwners);
                    //_dbContext.SaveChanges();
                }
                */
                if (!_dbContext.Categories.Any())
                {
                    var cat = getCat();
                    _dbContext.Categories.AddRange(cat);
                    _dbContext.SaveChanges();
                }
            }
        }

        private IEnumerable<Category> getCat()
        {
            var cat = new List<Category>()
            {
                new Category()
                {
                    Name = "Water"
                },
                new Category()
                {
                    Name = "Fire"
                },
                new Category()
                {
                    Name = "Earth"
                }
            };
            return cat;
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
