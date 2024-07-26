

namespace eFlow.Core.Models
{
    public class Bouquet
    {
        public Guid Id { get; set; }
        public string Name { get; set; } =  null!;
        public string Description { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public bool Available { get; set; } 
        public ICollection<Flower> Flowers { get; set; }
            = new List<Flower>();
        public ICollection<BouquetSize> Sizes { get; set; }
            = new List<BouquetSize>();
    }
}
