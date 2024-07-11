

namespace eFlow.Core.Models
{
    public class LoginResponse
    {
        public bool IsLogedIn { get; set; }
        public string? JwtToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
