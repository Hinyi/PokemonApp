using Microsoft.AspNetCore.Identity;
using PokemonApp.Data;
using PokemonApp.Entities;
using PokemonApp.Exceptions;
using PokemonApp.Interfaces;
using PokemonApp.Models.UserDto;

namespace PokemonApp.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly PokemonDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserRepository(PokemonDbContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }
        public void RegisterUser(RegisterUserDto dto)
        {
            if (!dto.Password.Equals(dto.ConfirmPassword))
            {
                throw new BadRequestException("Passwords are not matching!");
            }
            var newUser = new User()
            {
                Name = dto.Name,
                LastName= dto.LastName,
                Nickname= dto.Nickname,
                Email = dto.Email,
                RoleId= dto.RoleId
            };

            var hashedPassword = _passwordHasher.HashPassword(newUser, dto.Password);
            newUser.PasswordHash = hashedPassword;

            _context.Users.Add(newUser);
            _context.SaveChanges();
        }
    }
}
