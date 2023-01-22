using FluentValidation;
using PokemonApp.Data;
using PokemonApp.Exceptions;

namespace PokemonApp.Models.UserDto
{
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
    {
        private readonly PokemonDbContext _dbContext;

        public RegisterUserDtoValidator(PokemonDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(4)
                .MaximumLength(25);

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password);

            RuleFor(x => x.Email)
                .Custom((value, context) =>
                {
                    var emailInUse = _dbContext.Users.Any(x => x.Email == value);
                    if (emailInUse)
                    {
                        context.AddFailure("Email", "That email is taken");
                    }
                });

            RuleFor(x => x.Nickname)
                .Custom((value, context) =>
                {
                    var nicknameInUse = _dbContext.Users.Any(x => x.Nickname == value);
                    if (nicknameInUse)
                    {
                        //throw new BadRequestException("Nickname is already in use");
                        context.AddFailure("Nickname", "That nickname is taken");
                    }
                });
        }
    }
}
