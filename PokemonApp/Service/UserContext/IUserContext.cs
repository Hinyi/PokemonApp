using System.Security.Claims;

namespace PokemonApp.Service.UserContext
{
    public interface IUserContext
    {
        ClaimsPrincipal User { get; }
        int? GetUserId { get; }
    }
}
