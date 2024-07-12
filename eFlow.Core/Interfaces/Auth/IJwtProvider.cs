using eFlow.Core.Models;
using System.Security.Claims;

namespace eFlow.Core.Interfaces.Auth
{
    public interface IJwtProvider
    {
        public string GenerateToken(User user);
        public string GenerateRefreshToken();
        public ClaimsPrincipal? GetTokenPrincipal(string token);
    }
}
