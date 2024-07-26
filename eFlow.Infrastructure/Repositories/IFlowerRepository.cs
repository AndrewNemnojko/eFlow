
using eFlow.Core.Models;

namespace eFlow.Infrastructure.Repositories
{
    public interface IFlowerRepository
    {
        public Task<Guid> AddAsync(Flower flower);
        public Task<bool> RemoveAsync(Guid id);
        public Task<Flower?> GetByIdAsync(Guid id);
        Task<IEnumerable<Flower>> GetFlowersByIdsAsync(IEnumerable<Guid> flowerIds);
        public Task<Flower?> GetByNameAsync(string name);
        public Task<IList<Flower>> GetAllAsync();
        public Task<bool> UpdateAsync(Flower flower);
    }
}
