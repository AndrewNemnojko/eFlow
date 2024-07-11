
namespace eFlow.Persistence.Entities
{
    public class FlowerEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int InStock { get; set; }                
    }
}
