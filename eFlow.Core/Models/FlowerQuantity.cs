
namespace eFlow.Core.Models
{
    public class FlowerQuantity
    {
        public Guid Id { get; set; }
        public Flower Flower { get; set; } = null!;
        public int Quantity { get; set; }
    }
}
