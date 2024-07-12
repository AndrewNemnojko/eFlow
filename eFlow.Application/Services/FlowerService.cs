
using eFlow.Core.Models;
using eFlow.Infrastructure.Repositories;

namespace eFlow.Application.Services
{
    public class FlowerService
    {
        private readonly IFlowerRepository _flowerRepository;
        public FlowerService(IFlowerRepository flowerRepository)
        {
            _flowerRepository = flowerRepository;
        }
        public async Task<Guid> CreateFlowerAsync(Flower flower)
        {
            var data = await _flowerRepository.AddAsync(flower);
            return data;
        }
        public async Task<bool> UpdateFlowerAsync(Flower flower)
        {
            var data = await _flowerRepository.UpdateAsync(flower);
            return data;
        }
        public async Task<IList<Flower>> GetFlowersAsync()
        {
            var data = await _flowerRepository.GetAllAsync();
            return data;
        }
        public async Task<Flower> GetFlowerByName(string name)
        {
            var data = await _flowerRepository.GetByNameAsync(name);
            return data;
        }
        public async Task<Flower> GetFlowerById(Guid id)
        {
            var data = await _flowerRepository.GetByIdAsync(id);
            return data;
        }
        public async Task<bool> DeleteAsync(Guid id)
        {
            var data = await _flowerRepository.RemoveAsync(id);
            return data;
        }
    }
}
