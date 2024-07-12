using eFlow.Core.Interfaces.Caching;
using eFlow.Core.Models;
using eFlow.Infrastructure.Repositories;
using eFlow.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace eFlow.Persistence.Repositories
{
    public class FlowerRepository : IFlowerRepository
    {
        private readonly DataDbContext _dbContext;
        private readonly ICacheService _cacheService;
        public FlowerRepository(DataDbContext dataDbContext, ICacheService cacheService)
        {
            _cacheService = cacheService;
            _dbContext = dataDbContext;
        }

        public async Task<Guid> AddAsync(Flower flower)
        {
            var flowerEntity = new FlowerEntity
            {
                Id = Guid.NewGuid(),
                Name = flower.Name,
                Description = flower.Description,
                InStock = flower.InStock,
                Price = flower.Price,
            };
            await _dbContext.Flowers.AddAsync(flowerEntity);
            await _dbContext.SaveChangesAsync();
            await _cacheService
               .SetAsync($"flower:{flowerEntity.Id}", flowerEntity);
            return flowerEntity.Id;
        }

        public async Task<IList<Flower>> GetAllAsync()
        {
            return await _dbContext.Flowers.Select(x => new Flower
            {
                Id= x.Id,
                Name = x.Name,
                Description = x.Description,
                InStock = x.InStock,
                Price = x.Price,                
            }).ToListAsync();
        }

        public async Task<Flower?> GetByIdAsync(Guid id)
        {
            var cacheKey = $"flower:{id}";
            var cachedFlower = await _cacheService.GetAsync<Flower>(cacheKey);
            if (cachedFlower != null)
            {
                return cachedFlower;
            }
            var flowerEntity = await _dbContext.Flowers
                .FirstOrDefaultAsync(x => x.Id == id);

            if (flowerEntity == null)
                return null;

            var flower = new Flower
            {
                Id = flowerEntity.Id,
                Name = flowerEntity.Name,
                Description = flowerEntity.Description,
                InStock = flowerEntity.InStock,
                Price = flowerEntity.Price,
            };
            await _cacheService.SetAsync(cacheKey, flower);
            return flower;
        }

        public async Task<Flower?> GetByNameAsync(string name)
        {
            var cacheKey = $"flowerByName:{name}";
            var cachedFlower = await _cacheService.GetAsync<Flower>(cacheKey);
            if (cachedFlower != null)
            {
                return cachedFlower;
            }
            var flowerEntity = await _dbContext.Flowers
                .FirstOrDefaultAsync(x => x.Name == name);

            if (flowerEntity == null)
                return null;

            var flower = new Flower
            {
                Id = flowerEntity.Id,
                Name = flowerEntity.Name,
                Description = flowerEntity.Description,
                InStock = flowerEntity.InStock,
                Price = flowerEntity.Price,
            };
            await _cacheService.SetAsync(cacheKey, flower);
            return flower;
        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            var flowerEntity = await _dbContext.Flowers
                .FirstOrDefaultAsync(x => x.Id == id);

            if (flowerEntity == null)
                return false;

            _dbContext.Flowers.Remove(flowerEntity);
            await _dbContext.SaveChangesAsync();
            await _cacheService.RemoveAsync($"flower:{id}");
            return true;
        }

        public async Task<bool> UpdateAsync(Flower flower)
        {
            var flowerEntity = await _dbContext.Flowers
                .FirstOrDefaultAsync(x => x.Id == flower.Id);
            if (flowerEntity == null) return false;
            
            flowerEntity.Name = flower.Name;
            flowerEntity.Price = flower.Price;
            flowerEntity.Description = flower.Description;
            flowerEntity.InStock = flower.InStock;          

            await _dbContext.SaveChangesAsync();
            await _cacheService.SetAsync($"flower:{flowerEntity.Id}", flowerEntity);
            return true;
        }
    }
}
