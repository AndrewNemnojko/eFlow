
using eFlow.Core.Enum;

namespace eFlow.Core.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Adress { get; set; } = String.Empty;
        public string HashPassword { get; set; } = null!;
        public Role Role { get; set; } = Role.Member;
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpire { get; set; }
    }
}
