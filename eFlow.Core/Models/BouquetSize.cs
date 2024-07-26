
namespace eFlow.Core.Models
{
    public class BouquetSize
    {
        public Guid Id { get; set; }
        public string SubName { get; set; } = null!;
        public decimal BasePrice { get; set; }
        public bool Available { get; set; }
        public IList<MediaFile> ImageFile { get; set; }
            = new List<MediaFile>();
        public ICollection<FlowerQuantity> Flowers { get; set; }
            = new List<FlowerQuantity>();
    }
}
