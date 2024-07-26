
namespace eFlow.Persistence.Entities
{
    public class MediaFileEntity
    {
        public Guid Id { get; set; }
        public string FileName { get; set; } = null!;      
        public string FilePath { get; set; } = null!;
        public string ContentType { get; set; } = null!;
        public long Size { get; set; }
    }
}
