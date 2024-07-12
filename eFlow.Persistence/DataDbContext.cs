
using eFlow.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace eFlow.Persistence
{
    public class DataDbContext : DbContext
    {
        public DataDbContext(DbContextOptions<DataDbContext> options) 
            : base(options) 
        { }    
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<FlowerEntity> Flowers { get; set; }
        public DbSet<BouquetEntity> Bouquets { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {          
            base.OnModelCreating(builder);          
        }
    }
}
