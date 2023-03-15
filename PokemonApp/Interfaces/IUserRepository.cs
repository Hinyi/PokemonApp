using PokemonApp.Models.UserDto;

namespace PokemonApp.Interfaces
{
    public interface IUserRepository
    {
        void RegisterUser(RegisterUserDto dto);
        string GenerateJwt(LoginDto dto);
        void UpdateAddress(int id,UpdateAddressDto dto);
        Task UpdateGym(int id, UpdateUserGymDto dto);
        void DeleteUser();
    }
}
