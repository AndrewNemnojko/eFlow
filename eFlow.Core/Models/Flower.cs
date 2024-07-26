
namespace eFlow.Core.Models
{
    public class Flower
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public MediaFile? ImageFile { get; set; }
        public decimal Price { get; set; }
        public int InStock { get; set; }
    }
}
