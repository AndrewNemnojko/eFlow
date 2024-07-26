
namespace eFlow.Application.DTOs.Bouquets
{
    public record BouquetSizeDTO
    (Guid id, string SubName, ICollection<FlowerQuantityDTO> Flowers);
}
