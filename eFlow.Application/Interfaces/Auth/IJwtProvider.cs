using eFlow.Core.Models;
using System.Security.Claims;

namespace eFlow.Application.Interfaces.Auth
{
    public interface IJwtProvider
    {
        public string GenerateToken(User user);
        public string GenerateRefreshToken();
        public ClaimsPrincipal? GetTokenPrincipal(string token);
    }
}
