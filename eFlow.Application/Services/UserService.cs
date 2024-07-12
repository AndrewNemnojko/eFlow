
using eFlow.Core.Models;
using eFlow.Infrastructure.Repositories;

namespace eFlow.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IList<User>> GetUsersAsync()
        {
            var data = await _userRepository.GetAllAsync();
            return data;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var data = await _userRepository.GetByEmailAsync(email);
            return data;
        }
    }
}
