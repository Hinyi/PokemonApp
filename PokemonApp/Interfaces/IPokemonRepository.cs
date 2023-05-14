﻿using PokemonApp.Entities;
using PokemonApp.Models.PokemonDto;

namespace PokemonApp.Interfaces
{
    public interface IPokemonRepository
    {
        bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon);
        bool CreatePokemon(int categoryId,Pokemon pokemon);
        ICollection<Pokemon> GetPokemons();
        public Pokemon GetPokemonTrimToUpper(PokemonDto pokemonCreate);
        bool Save();
    }
}
