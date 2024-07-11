using eFlow.Application.Interfaces.Caching;
using eFlow.Application.Interfaces.Repositories;
using eFlow.Core.Models;
using eFlow.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace eFlow.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataDbContext _dbContext;
        private readonly ICacheService _cacheService;
        public UserRepository(DataDbContext dataDbContext, ICacheService cacheService)
        {
            _dbContext = dataDbContext;
            _cacheService = cacheService;
        }
        public async Task AddAsync(User user)
        {
            var userEntity = new UserEntity
            {
                Id = user.Id,
                Name = user.Name,
                Adress = user.Adress,
                Email = user.Email,
                HashPassword = user.HashPassword,
                Role = user.Role,
            };
            await _dbContext.Users.AddAsync(userEntity);
            await _dbContext.SaveChangesAsync();
            await _cacheService
                .SetAsync($"user:{userEntity.Id}", userEntity);
        }

        public async Task<IList<User>> GetAllAsync()
        {
            return await _dbContext.Users
                .Select(x => 
                    new User { 
                        Id = x.Id, 
                        Name = x.Name, 
                        Email = x.Email, 
                        Adress = x.Adress, 
                        HashPassword = x.HashPassword, 
                        Role = x.Role,                       
                    })
                .ToListAsync();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            var cacheKey = $"userByEmail:{email}";
            var cachedUser = await _cacheService.GetAsync<User>(cacheKey);
            if (cachedUser != null)
            {
                return cachedUser;
            }

            var userEntity = await _dbContext.Users
                .FirstOrDefaultAsync(x => x.Email == email);
            
            if (userEntity == null)
                return null;

            var user = new User
            {
                Id = userEntity.Id,
                Name = userEntity.Name,
                Adress = userEntity.Adress,
                Role = userEntity.Role,
                Email = userEntity.Email,
                HashPassword = userEntity.HashPassword,
                RefreshToken = userEntity.RefreshToken,
                RefreshTokenExpire = userEntity.RefreshTokenExpire,
            };

            await _cacheService.SetAsync(cacheKey, user);
            return user;
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            var cacheKey = $"user:{id}";
            var cachedUser = await _cacheService.GetAsync<User>($"user:{id}");
            if (cachedUser != null)
            {
                return cachedUser;
            }
            var userEntity = await _dbContext.Users
                .FirstOrDefaultAsync(x => x.Id == id);

            if (userEntity == null)
                return null;

            var user = new User
            {
                Id = userEntity.Id,
                Name = userEntity.Name,
                Adress = userEntity.Adress,
                Email = userEntity.Email,
                HashPassword = userEntity.HashPassword,
            };
            await _cacheService.SetAsync(cacheKey, user);
            return user;
        }
    
        public async Task<bool> RemoveAsync(Guid id)
        {
            var userEntity = await _dbContext.Users
                .FirstOrDefaultAsync(x => x.Id == id);
            
            if (userEntity == null)
                return false;

            _dbContext.Users.Remove(userEntity);
            await _dbContext.SaveChangesAsync();
            await _cacheService.RemoveAsync($"user:{id}");
            return true;
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            var userEntity = await _dbContext.Users
                .FirstOrDefaultAsync(x => x.Id ==  user.Id);
            if(userEntity == null) return false;   
            userEntity.Email = user.Email;
            userEntity.Adress = user.Adress;
            userEntity.Role = user.Role;
            userEntity.RefreshToken = user.RefreshToken;
            userEntity.RefreshTokenExpire = user.RefreshTokenExpire;
            userEntity.Name = user.Name;
            userEntity.HashPassword = user.HashPassword;

            await _dbContext.SaveChangesAsync();
            await _cacheService.SetAsync($"user:{user.Id}", userEntity);
            return true;
        }
    }
}
