using System.ComponentModel.DataAnnotations;

namespace eFlow.API.Contracts
{
    public record RegisterUserRequest(
        [Required]string email,
        [Required] string password, 
        [Required] string username);
}
