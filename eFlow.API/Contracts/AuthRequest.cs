using System.ComponentModel.DataAnnotations;

namespace eFlow.API.Contracts
{
    public record AuthRequest(
        [Required] string email, 
        [Required] string password);
}
