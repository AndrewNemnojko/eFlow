namespace eFlow.Application.DTOs
{
    public record FlowerDTO
    (
        Guid id,
        string Name,
        decimal Price,
        int InStock,
        string Description
    );
}
