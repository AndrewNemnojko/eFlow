namespace eFlow.API.DTOs.Flowers
{
    public record FlowerDTO
    (
        string Name,
        decimal Price,
        int InStock,
        string Description
    );
}
