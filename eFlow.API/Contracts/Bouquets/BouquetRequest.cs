using eFlow.Application.DTOs.Bouquets;
using eFlow.Core.Models;

namespace eFlow.API.Contracts.Bouquets
{
    public record BouquetRequest
    (      
        Guid Id,
        string Name,                     
        ICollection<BouquetSizeDTO> Sizes
    );
}
