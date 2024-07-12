using eFlow.Core.Models;

namespace eFlow.Infrastructure.Repositories
{
    public interface IUserRepository
    {
        public Task AddAsync(User user);
        public Task<bool> RemoveAsync(Guid id);
        public Task<User?> GetByIdAsync(Guid id);
        public Task<User?> GetByEmailAsync(string email);
        public Task<IList<User>> GetAllAsync();
        public Task<bool> UpdateUserAsync(User user);
    }
}
