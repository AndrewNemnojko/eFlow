using System.ComponentModel.DataAnnotations;

namespace eFlow.API.Contracts.Auth
{
    public record AuthRequest
    (
        [Required] string email,
        [Required] string password
    );
}
