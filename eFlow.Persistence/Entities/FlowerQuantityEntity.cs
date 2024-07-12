

namespace eFlow.Persistence.Entities
{
    public class FlowerQuantityEntity
    {
        public FlowerEntity Flower { get; set; } = null!;
        public int Quantity { get; set; }
    }
}
