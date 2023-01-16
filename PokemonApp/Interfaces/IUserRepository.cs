using PokemonApp.Models.UserDto;

namespace PokemonApp.Interfaces
{
    public interface IUserRepository
    {
        void RegisterUser(RegisterUserDto dto);
    }
}
