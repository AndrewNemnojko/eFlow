using eFlow.Application.Interfaces.Auth;
using eFlow.Application.Interfaces.Repositories;
using eFlow.Core.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace eFlow.Application.Services
{
    public class AuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtProvider _jwtProvider;
        public AuthService(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IJwtProvider jwtProvider)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
        }
        public async Task<LoginResponse> Login(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);

            if (user == null || !_passwordHasher.Verify(password, user.HashPassword))
                return new LoginResponse() { IsLogedIn = false };

            var response = new LoginResponse
            {
                IsLogedIn = true,
                JwtToken = _jwtProvider.GenerateToken(user),
                RefreshToken = _jwtProvider.GenerateRefreshToken(),
            };

            user.RefreshToken = response.RefreshToken;
            user.RefreshTokenExpire = DateTime.UtcNow.AddDays(7);

            await _userRepository.UpdateUserAsync(user);
            return response;
        }

        public async Task Registration(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                await _userRepository.AddAsync(new Core.Models.User
                {
                    Id = Guid.NewGuid(),
                    Name = "Name",
                    Email = email,
                    Adress = "Earth",
                    HashPassword = _passwordHasher
                        .Generate(password)
                });
            }
        }

        public async Task<LoginResponse> RefreshToken(RefreshTokenModel model)
        {
            var principal = _jwtProvider.GetTokenPrincipal(model.JwtToken);
            
            var email = principal?.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            if (email == null)
                return new LoginResponse{IsLogedIn = false};

            var user = await _userRepository.GetByEmailAsync(email);
            
            if (user is null || user.RefreshToken != model.RefreshToken || user.RefreshTokenExpire < DateTime.Now)
                return new LoginResponse { IsLogedIn = false };

            var response = new LoginResponse
                {
                    IsLogedIn = true,
                    JwtToken = _jwtProvider.GenerateToken(user),
                    RefreshToken = _jwtProvider.GenerateRefreshToken()
                };
            user.RefreshToken = response.RefreshToken;
            user.RefreshTokenExpire = DateTime.UtcNow.AddDays(7);
            await _userRepository.UpdateUserAsync(user);

            return response;
        }
      
    }
}
