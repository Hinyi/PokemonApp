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

        public async Task<PagedResult<GetAllPokemonsPaginated>> GetAllPokemonsPaged(GetPokemons query)
        {
            var (pageNumber, pageSize, sortDirection) = query;
            //var pokemons = _context.Pokemons
            //    .Include(x => x.PokemonCategories)
            //    //.Include(x=> x.)
            //    .Select(x => new GetAllPokemonsPaginated
            //    {
            //        Id = x.Id,
            //        Name = x.Name,
            //        Categories = x.PokemonCategories
            //    })
            //    .AsNoTracking()
            //    .AsQueryable();

            //var pokemonsWithCategory =await _context.PokemonCategories
            //    .Where(x => x.PokemonId == pokemons.ToListAsync);
            //var pokemons1 = _context.PokemonCategories
            //    .Include(x => x.Category)
            //    .Include(x => x.Pokemon)
            //    .Select(x => new GetAllPokemonsPaginated
            //    {
            //        Name = x.Pokemon.Name,
            //        Id = x.Pokemon.Id,
            //        Categories = x.Category.Name,
            //    })
            //    .AsNoTracking()
            //    .AsQueryable();

            //var pokemonGroup = _context.Pokemons
            //    .GroupJoin(_context.PokemonCategories,
            //        pokemon => pokemon.Id,
            //        pokemonCategory => pokemonCategory.PokemonId,
            //        (pokemon, pokemonCategories) => new
            //        {
            //            Id = pokemon.Id,
            //            Name = pokemon.Name,

            //            Categories = _context.Categories.Join(pokemonCategories,
            //                category => category.Id,
            //                pokemonCategory => pokemonCategory.CategoryId,
            //                (category, pokemonCategoriesThis) => new
            //                {
            //                    Id = category.Id,
            //                    Name = category.Name,
            //                })
            //        })
            //    .AsNoTracking()
            //    .AsQueryable();

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

            //PagedResult<GetAllPokemonsPaginated> result = null;
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
