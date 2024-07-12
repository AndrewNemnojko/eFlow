
namespace eFlow.Persistence.Entities
{
    public class BouquetEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public bool Available { get; set; }
        public ICollection<FlowerEntity> Flowers { get; set; } 
            = new List<FlowerEntity>();
    }
}
