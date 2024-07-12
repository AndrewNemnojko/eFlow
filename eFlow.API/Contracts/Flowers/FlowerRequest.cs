using System.ComponentModel.DataAnnotations;

namespace eFlow.API.Contracts.Flowers
{
    public record FlowerRequest
    (
        [Required] string Name,
        [Required] decimal Price,
        [Required] int InStock,
        string? Description,
        IFormFile? ImageFile
    );
}
