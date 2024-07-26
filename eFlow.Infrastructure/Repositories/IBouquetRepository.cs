
using eFlow.Core.Models;

namespace eFlow.Infrastructure.Repositories
{
    public interface IBouquetRepository
    {
        public Task<Guid> AddAsync(Bouquet bouquet, IList<Guid> FlowerIds);
        public Task<bool> RemoveAsync(Guid id);
        public Task<Bouquet?> GetByIdAsync(Guid id);
        public Task<Bouquet?> GetByNameAsync(string name);
        public Task<IList<Bouquet>> GetAllAsync();
        public Task<IList<Bouquet>> GetByIdAsync(Guid[] Ids);
        public Task<bool> ChangeAsync(Bouquet bouquet, IList<Guid> FlowerIds);
        public Task<bool> UpdateAsync(IList<Bouquet> Bouquets);
    }
}
