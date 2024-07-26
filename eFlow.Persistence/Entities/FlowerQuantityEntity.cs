

namespace eFlow.Persistence.Entities
{
    public class FlowerQuantityEntity
    {
        public Guid ID { get; set; }
        public FlowerEntity Flower { get; set; } = null!;
        public int Quantity { get; set; }
    }
}
