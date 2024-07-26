
using eFlow.API.Contracts.Auth;
using eFlow.Application.Services;
using eFlow.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace eFlow.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(AuthRequest login)
        {
            try
            {
                var result = await _authService.Login(login.email, login.password);
                if (!result.IsLogedIn)
                {
                    return BadRequest("Failed to login");
                }
                return Ok(
                    new AuthResponse
                    (result.JwtToken!,
                    result.RefreshToken!));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Registration(AuthRequest login)
        {
            try
            {
                await _authService.Registration(login.email, login.password);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }    
        }      

        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenModel model)
        {
            try
            {
                var data = await _authService.RefreshToken(model);
                if (!data.IsLogedIn)
                {
                    return BadRequest("Failed to update access token");
                }
                return Ok(new AuthResponse(data.JwtToken!, data.RefreshToken!));
            }          
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
