
namespace eFlow.Core.Models
{
    public class RefreshTokenModel
    {
        public string JwtToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}
