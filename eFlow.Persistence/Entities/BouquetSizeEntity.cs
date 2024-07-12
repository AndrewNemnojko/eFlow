
namespace eFlow.Persistence.Entities
{
    public class BouquetSizeEntity
    {
        public Guid Id { get; set; }
        public string SubName { get; set; } = null!;       
        public decimal BasePrice { get; set; }
        public bool Available { get; set; }
        public ICollection<FlowerQuantityEntity> Flowers { get; set; }
            = new List<FlowerQuantityEntity>();
    }
}
