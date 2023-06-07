using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using PokemonApp.Data;
using PokemonApp.Entities;
using PokemonApp.Exceptions;
using PokemonApp.Helper;
using PokemonApp.Interfaces;
using PokemonApp.Models.PokemonDto;
using PokemonApp.Service.UserContext;

namespace PokemonApp.Repository
{
    public record GetPokemons(uint PageNumber, uint PageSize, SortDirection SortDirection)
        : QueryForPaginatedReesult(PageNumber, PageSize);
    public class PokemonRepository : IPokemonRepository
    {
        private readonly PokemonDbContext _context;
        private readonly IUserContext _userContextService;
        private readonly IMapper _mapper;

        public PokemonRepository(PokemonDbContext context
            , IUserContext userContextService, IMapper mapper)
        {
            _context = context;
            _userContextService = userContextService;
            _mapper = mapper;
        }

        public bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon)
        {
            var pokemonOwnerEntity = _context.Users.FirstOrDefault(a => a.Id == ownerId);
            var category = _context.Categories.FirstOrDefault(x => x.Id == categoryId);


            var newPokemon = new Pokemon()
            {
                Name = pokemon.Name,
                DateOfBirth = DateTime.Now,
            };

            var pokemonCategory = new PokemonCategory()
            {
                Category = category,
                Pokemon = newPokemon,
            };

            var pokemonOwner = new PokemonUser()
            {
                User = pokemonOwnerEntity,
                Pokemon = newPokemon,

            };

            _context.Add(pokemonCategory);
            _context.Add(pokemonOwner);
            _context.Add(newPokemon);

            return Save();
        }

        public bool CreatePokemon(string cat, Pokemon pokemon)
        {
            var pokemonOwnerEntity = _context.Users.FirstOrDefault(a => a.Id == _userContextService.GetUserId);
            var category = _context.Categories.FirstOrDefault(x => x.Name == cat);

            if (pokemonOwnerEntity == null)
                throw new NotFoundException("User is not logged in!");

            var newPokemon = new Pokemon()
            {
                Name = pokemon.Name,
                DateOfBirth = DateTime.Now,
            };

            var pokemonCategory = new PokemonCategory()
            {
                Category = category,
                Pokemon = newPokemon,
            };

            var pokemonOwner = new PokemonUser()
            {
                User = pokemonOwnerEntity,
                Pokemon = newPokemon,

            };

            _context.Add(pokemonCategory);
            _context.Add(pokemonOwner);
            _context.Add(newPokemon);

            return Save();
        }

        public ICollection<Pokemon> GetPokemons()
        {
            return _context.Pokemons.OrderBy(x => x.Id).ToList();
        }

        public Pokemon GetPokemon(int id)
        {
            return _context.Pokemons.FirstOrDefault(x => x.Id == id);
        }
        public Pokemon GetPokemon(string name)
        {
            return _context.Pokemons.FirstOrDefault(x => x.Name == name);
        }

        public async Task<PagedResult<GetAllPokemonsPaginated>> GetAllPokemonsPaged(GetPokemons query)
        {
            var (pageNumber, pageSize, sortDirection) = query;

            var pokemons1 = _context.Pokemons
                .Select(pokemons => new GetAllPokemonsPaginated()
                {
                    Id = pokemons.Id,
                    Name = pokemons.Name,
                    Categories = pokemons.Categories.Select(category => new
                    {
                        CategoryName = category.Name,
                    })
                })
                .AsNoTracking()
                .AsQueryable();

            var orderBy = SortDirection.Ascending == sortDirection
                ? pokemons1.OrderBy(x => x.Id)
                : pokemons1.OrderByDescending(x => x.Id);

            var result = await PagedResult<GetAllPokemonsPaginated>
                .CreateAsync(orderBy, pageNumber, pageSize);
            return result;
        }

        public Pokemon GetPokemonTrimToUpper(PokemonDto pokemonCreate)
        {
            return GetPokemons().Where(c => c.Name.Trim().ToUpper() == pokemonCreate.Name.TrimEnd().ToUpper())
                .FirstOrDefault();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
