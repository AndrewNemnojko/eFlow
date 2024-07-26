
using eFlow.Core.Interfaces.Caching;
using eFlow.Core.Models;
using eFlow.Infrastructure.Repositories;
using eFlow.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace eFlow.Persistence.Repositories
{
    public class BouquetRepository : IBouquetRepository
    {
        private readonly DataDbContext _dbContext;
        private readonly ICacheService _cacheService;
        public BouquetRepository(DataDbContext dataDbContext, ICacheService cacheService) 
        {
            _cacheService = cacheService;
            _dbContext = dataDbContext;
        }

        public async Task<Guid> AddAsync(Bouquet bouquet, IList<Guid> FlowerIds)
        {
            var flowers = await _dbContext.Flowers
                .Where(f => FlowerIds.Contains(f.Id))
                .ToListAsync();

            var bouquetEntity = new BouquetEntity
            {
                Id = bouquet.Id,
                Name = bouquet.Name,
                Available = bouquet.Available,
                BasePrice = bouquet.BasePrice,
                Description = bouquet.Description,
                Flowers = flowers,
                Sizes = bouquet.Sizes.Select(s => new BouquetSizeEntity
                {
                    Id = s.Id,
                    Available = s.Available,
                    BasePrice = s.BasePrice,
                    SubName = s.SubName,
                    Flowers = s.Flowers.Select(fs => new FlowerQuantityEntity
                    {
                        Flower = flowers.First(x => x.Id == fs.Flower.Id),
                        Quantity = fs.Quantity,
                    }).ToList(),
                }).ToList(),
            };          

            await _dbContext.Bouquets.AddAsync(bouquetEntity);
            await _dbContext.SaveChangesAsync();           
            return bouquetEntity.Id;
        }

        public async Task<IList<Bouquet>> GetAllAsync()
        {
            return await _dbContext.Bouquets.Select(x => new Bouquet
            {
                Id= x.Id,
                Name= x.Name,
                BasePrice= x.BasePrice,
                Description = x.Description,
                Available= x.Available,              
            }).ToListAsync();
        }

        public async Task<Bouquet?> GetByIdAsync(Guid id)
        {
            var cacheKey = $"bouquet:{id}";
            var cachedBouquet = await _cacheService.GetAsync<Bouquet>(cacheKey);
            if (cachedBouquet != null)
            {
                return cachedBouquet;
            }

            var data = await _dbContext.Bouquets
                .Include(b => b.Sizes)
                    .ThenInclude(f => f.Flowers)
                    .ThenInclude(x => x.Flower)
                .Include(f => f.Flowers)
                .FirstOrDefaultAsync(b => b.Id == id);
            if (data == null)
                return null;

            var bouquet = new Bouquet
            {
                Id = data.Id,
                Name = data.Name,
                Available = data.Available,
                BasePrice = data.BasePrice,
                Description = data.Description,
                Sizes = data.Sizes.Select(bs => new BouquetSize
                {
                    Id = bs.Id,
                    Available = bs.Available,
                    BasePrice = bs.BasePrice,
                    SubName = bs.SubName,
                    Flowers = bs.Flowers.Select(fq => new FlowerQuantity
                    {
                        Id = fq.ID,
                        Flower = new Flower
                        { Id = fq.Flower.Id, Name = fq.Flower.Name },
                        Quantity = fq.Quantity,
                    }).ToList()
                }).ToList(),
                Flowers = data.Flowers.Select(x => new Flower
                {
                    Id=x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    InStock = x.InStock,
                    Price = x.Price,
                }).ToList(),
            };
            await _cacheService.SetAsync(cacheKey, bouquet);
            return bouquet;
        }

        public async Task<IList<Bouquet>> GetByIdAsync(Guid[] Ids)
        {
            var data = await _dbContext.Bouquets
                .AsNoTracking()
                 .Include(b => b.Sizes)
                     .ThenInclude(f => f.Flowers)
                     .ThenInclude(x => x.Flower)
                 .Include(f => f.Flowers)
                 .Where(b => Ids.Contains(b.Id))
                 .ToListAsync();

            if (!data.Any())
            {
                return new List<Bouquet>();
            }
            var bouquets = data.Select(x => new Bouquet
            {
                Id = x.Id,
                Name = x.Name,
                Available = x.Available,
                BasePrice = x.BasePrice,
                Description = x.Description,
                Sizes = x.Sizes.Select(bs => new BouquetSize
                {
                    Id = bs.Id,
                    Available = bs.Available,
                    BasePrice = bs.BasePrice,
                    SubName = bs.SubName,
                    Flowers = bs.Flowers.Select(fq => new FlowerQuantity
                    {
                        Id = fq.ID,
                        Flower = new Flower
                            { 
                                Id = fq.Flower.Id, 
                                Name = fq.Flower.Name, 
                                InStock = fq.Flower.InStock, 
                                Price = fq.Flower.Price 
                            },
                        Quantity = fq.Quantity,
                    }).ToList()
                }).ToList(),
                Flowers = x.Flowers.Select(x => new Flower
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    InStock = x.InStock,
                    Price = x.Price,
                }).ToList(),
            }).ToList();
            return bouquets;
        }

        public async Task<Bouquet?> GetByNameAsync(string name)
        {
            
            var cacheKey = $"bouquetByName:{name}";
            var cachedBouquet = await _cacheService.GetAsync<Bouquet>(cacheKey);
            if (cachedBouquet != null)
            {
                return cachedBouquet;
            }

            var data = await _dbContext.Bouquets
                .Include(b => b.Sizes)
                    .ThenInclude(f => f.Flowers)
                    .ThenInclude(x => x.Flower)
                .Include(f => f.Flowers)
                .FirstOrDefaultAsync(b => b.Name == name);
            if (data == null)
                return null;

            var bouquet = new Bouquet
            {
                Id = data.Id,
                Name = data.Name,
                Available = data.Available,
                BasePrice = data.BasePrice,
                Description = data.Description,
                Sizes = data.Sizes.Select(bs => new BouquetSize
                {
                    Id = bs.Id,
                    Available = bs.Available,
                    BasePrice = bs.BasePrice,
                    SubName = bs.SubName,
                    Flowers = bs.Flowers.Select(fq => new FlowerQuantity
                    {
                        Id = fq.ID,
                        Flower = new Flower
                        { Id = fq.Flower.Id, Name = fq.Flower.Name },
                        Quantity = fq.Quantity,
                    }).ToList()
                }).ToList(),
            };
            await _cacheService.SetAsync(cacheKey, bouquet);
            return bouquet;
        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            var bouquetEntity = await _dbContext.Bouquets
                .Include(b => b.Sizes)
                    .ThenInclude(f => f.Flowers)
                    .ThenInclude(x => x.Flower)
                .Include(f => f.Flowers)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (bouquetEntity == null)
                return false;

            _dbContext.Bouquets.Remove(bouquetEntity);
            await _dbContext.SaveChangesAsync();
            await _cacheService.RemoveAsync($"bouquet:{id}");
            return true;
        }

        public async Task<bool> ChangeAsync(Bouquet bouquet, IList<Guid> FlowerIds)
        {
            var bouquetEntity = await _dbContext.Bouquets
                .Include(b => b.Sizes)
                    .ThenInclude(f => f.Flowers)
                    .ThenInclude(x => x.Flower)
                .Include(f => f.Flowers)
                .FirstOrDefaultAsync(x => x.Id == bouquet.Id);

            if (bouquetEntity == null) return false;

            var flowers = await _dbContext.Flowers
                .Include(f => f.Bouquets)
                .Where(f => FlowerIds.Contains(f.Id))
                .ToListAsync();

            bouquetEntity.Name = bouquet.Name;
            bouquetEntity.Available = bouquet.Available;
            bouquetEntity.BasePrice = bouquet.BasePrice;
            bouquetEntity.Description = bouquet.Description;
            bouquetEntity.Flowers = flowers;

            var existingSizes = bouquetEntity.Sizes;

            var sizesToRemove = existingSizes
                .Where(es => !bouquet.Sizes.Any(ns => ns.Id == es.Id)).ToList();

            foreach (var sizeToRemove in sizesToRemove)
                existingSizes.Remove(sizeToRemove);

            foreach (var newSize in bouquet.Sizes)
            {
                var existingSize = existingSizes.FirstOrDefault(es => es.Id == newSize.Id);

                if (existingSize != null)
                {
                    existingSize.SubName = newSize.SubName;
                    existingSize.Available = newSize.Available;
                    existingSize.BasePrice = newSize.BasePrice;

                    var existingFlowers = existingSize.Flowers;

                    var flowersToRemove = existingFlowers
                        .Where(ef => !newSize.Flowers.Any(nf => nf.Flower.Id == ef.Flower.Id)).ToList();

                    foreach (var flowerToRemove in flowersToRemove)
                        existingFlowers.Remove(flowerToRemove);

                    foreach (var newFlower in newSize.Flowers)
                    {
                        var existingFlower = existingFlowers
                            .FirstOrDefault(ef => ef.Flower.Id == newFlower.Flower.Id);

                        if (existingFlower != null)
                            existingFlower.Quantity = newFlower.Quantity;


                        else
                        {
                            existingFlowers.Add(new FlowerQuantityEntity
                            {
                                Flower = flowers.First(f => f.Id == newFlower.Flower.Id),
                                Quantity = newFlower.Quantity
                            });
                        }
                    }
                }
                else
                {
                    var newSizeEntity = new BouquetSizeEntity
                    {
                        SubName = newSize.SubName,
                        Available = newSize.Available,
                        BasePrice = newSize.BasePrice,
                        Flowers = newSize.Flowers.Select(fs => new FlowerQuantityEntity
                        {
                            Flower = flowers.First(f => f.Id == fs.Flower.Id),
                            Quantity = fs.Quantity
                        }).ToList()
                    };
                    existingSizes.Add(newSizeEntity);
                }
            }

            _dbContext.Bouquets.Update(bouquetEntity);

            await _dbContext.SaveChangesAsync();
            return true;

        }

        public async Task<bool> UpdateAsync(IList<Bouquet> bouquets)
        {           
            var bouquetIds = bouquets.Select(b => b.Id).ToList();
      
            var existingBouquets = await _dbContext.Bouquets
                .Include(b => b.Sizes)
                    .ThenInclude(s => s.Flowers)
                        .ThenInclude(fq => fq.Flower)
                .Where(b => bouquetIds.Contains(b.Id))
                .ToListAsync();

            if (existingBouquets.Count != bouquetIds.Count)
                return false;
                     
            var bouquetDictionary = existingBouquets.ToDictionary(b => b.Id);

            foreach (var bouquet in bouquets)
            {
                if (bouquetDictionary.TryGetValue(bouquet.Id, out var bouquetEntity))
                {
                    foreach (var size in bouquet.Sizes)
                    {
                        var sizeEntity = bouquetEntity.Sizes
                            .FirstOrDefault(s => s.Id == size.Id);

                        if (sizeEntity != null)
                        {
                            sizeEntity.BasePrice = size.BasePrice;
                            sizeEntity.Available = size.Available;     
                        }
                    }
                    bouquetEntity.BasePrice = bouquet.Sizes
                        .Min(s => s.BasePrice);

                    bouquetEntity.Available = bouquet.Sizes
                        .Any(s => s.Available);
                }
            }

            _dbContext.Bouquets.UpdateRange(existingBouquets);
            await _dbContext.SaveChangesAsync();

            return true;
        }


    }
}
