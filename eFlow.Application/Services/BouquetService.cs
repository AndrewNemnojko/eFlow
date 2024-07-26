
using eFlow.Core.Models;
using eFlow.Infrastructure.Repositories;

namespace eFlow.Application.Services
{
    public class BouquetService
    {
        private readonly IBouquetRepository _bouquetRepository;
        private readonly IFlowerRepository _flowerRepository;
        public BouquetService(
            IBouquetRepository bouquetRepository, 
            IFlowerRepository flowerRepository)
        {
            _bouquetRepository = bouquetRepository;
            _flowerRepository = flowerRepository;
        }

        public async Task<Guid> CreateBouquetAsync(Bouquet bouquetDto)
        {
            var flowerIds = bouquetDto.Sizes
                .SelectMany(size => size.Flowers)
                .Select(fq => fq.Flower.Id)
                .Distinct().ToList();

            var flowerEntities = await _flowerRepository
                .GetFlowersByIdsAsync(flowerIds);

            var bouquet = new Bouquet
            {
                Id = Guid.NewGuid(), Name = bouquetDto.Name,
                Description = bouquetDto.Description,
                Available = false,         
            };

            decimal? minPrice = null;
            var sizes = new List<BouquetSize>();

            foreach (var sizeDto in bouquetDto.Sizes)
            {
                var sizeFlowers = sizeDto.Flowers.Select(fqDto => new FlowerQuantity
                {
                    Flower = flowerEntities.First(f => f.Id == fqDto.Flower.Id),
                    Quantity = fqDto.Quantity
                }).ToList();

                var sizeAvailable = sizeFlowers
                    .All(fq => fq.Quantity <= fq.Flower.InStock);

                var sizeCost = sizeFlowers
                    .Sum(fq => fq.Flower.Price * fq.Quantity);

                var size = new BouquetSize
                { 
                    Id = Guid.NewGuid(), SubName = sizeDto.SubName,
                    BasePrice = sizeCost, Available = sizeAvailable,
                    Flowers = sizeFlowers
                };

                if (sizeAvailable)
                    bouquet.Available = true;

                if (minPrice == null || sizeCost < minPrice)
                    minPrice = sizeCost;

                sizes.Add(size);
            }

            bouquet.Sizes = sizes;
            bouquet.BasePrice = minPrice ?? 0;
            
            var addedBouquet = await _bouquetRepository.AddAsync(bouquet, flowerIds);
            return addedBouquet;
        }
        public async Task<bool> ChangeBouquetAsync(Bouquet bouquetDto)
        {
            var flowerIds = bouquetDto.Sizes
                .SelectMany(size => size.Flowers)
                .Select(fq => fq.Flower.Id)
                .Distinct().ToList();

            var flowerEntities = await _flowerRepository                
                .GetFlowersByIdsAsync(flowerIds);

            var bouquet = new Bouquet
            {
                Id = bouquetDto.Id,
                Name = bouquetDto.Name,
                Description = bouquetDto.Description,
                Available = false,
            };

            decimal? minPrice = null;
            var sizes = new List<BouquetSize>();

            foreach (var sizeDto in bouquetDto.Sizes)
            {
                var sizeFlowers = sizeDto.Flowers.Select(fqDto => new FlowerQuantity
                {
                    Id=fqDto.Id,
                    Flower = flowerEntities.First(f => f.Id == fqDto.Flower.Id),
                    Quantity = fqDto.Quantity
                }).ToList();

                var sizeAvailable = sizeFlowers
                    .All(fq => fq.Quantity <= fq.Flower.InStock);

                var sizeCost = sizeFlowers
                    .Sum(fq => fq.Flower.Price * fq.Quantity);

                var size = new BouquetSize
                {
                    Id = sizeDto.Id,
                    SubName = sizeDto.SubName,
                    BasePrice = sizeCost,
                    Available = sizeAvailable,
                    Flowers = sizeFlowers
                };

                if (sizeAvailable)
                    bouquet.Available = true;

                if (minPrice == null || sizeCost < minPrice)
                    minPrice = sizeCost;

                sizes.Add(size);
            }

            bouquet.Sizes = sizes;
            bouquet.BasePrice = minPrice ?? 0;

            var addedBouquet = await _bouquetRepository.ChangeAsync(bouquet, flowerIds);
            return addedBouquet;
        }
        public async Task<bool> UpdateBouquetAsync(Guid[] guids)
        {
            var bouquets = await _bouquetRepository.GetByIdAsync(guids);
            if (bouquets == null || !bouquets.Any())
            {
                return false;
            }
            foreach (var bouquet in bouquets)
            {
                decimal? minPrice = null;
                var sizes = new List<BouquetSize>();

                foreach (var size in bouquet.Sizes)
                {
                    var sizeFlowers = size.Flowers.Select(fq => new FlowerQuantity
                    {
                        Id = fq.Id,
                        Flower = fq.Flower,
                        Quantity = fq.Quantity
                    }).ToList();

                    var sizeAvailable = sizeFlowers
                        .All(fq => fq.Quantity <= fq.Flower.InStock);

                    var sizeCost = sizeFlowers
                        .Sum(fq => fq.Flower.Price * fq.Quantity);

                    size.BasePrice = sizeCost;
                    size.Available = sizeAvailable;
                    size.Flowers = sizeFlowers;

                    if (sizeAvailable)
                        bouquet.Available = true;

                    if (minPrice == null || sizeCost < minPrice)
                        minPrice = sizeCost;

                    sizes.Add(size);
                }

                bouquet.Sizes = sizes;
                bouquet.BasePrice = minPrice ?? 0;
            }

            var updated = await _bouquetRepository.UpdateAsync(bouquets);
            return updated;
        }
        public async Task<IList<Bouquet>> GetBouquetsAsync()
        {
            var data = await _bouquetRepository.GetAllAsync();
            return data;
        }
        public async Task<Bouquet> GetBouquetByName(string name)
        {
            var data = await _bouquetRepository.GetByNameAsync(name);
            return data;
        }
        public async Task<Bouquet> GetBouquetById(Guid id)
        {
            var data = await _bouquetRepository.GetByIdAsync(id);
            return data;
        }
        public async Task<bool> DeleteAsync(Guid id)
        {
            var data = await _bouquetRepository.RemoveAsync(id);
            return data;
        }
    }
}
