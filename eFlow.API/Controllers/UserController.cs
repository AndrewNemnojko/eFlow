
using eFlow.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace eFlow.API.Controllers
{
    [ApiController]
    [Route("users")]  
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("accounts")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllAccounts()
        {          
            var result = await _userService.GetUsersAsync();
            return Ok(result);
        }
        [HttpGet]
        [Route("profile/{email}")]
        [Authorize(Roles = "Manager, Admin")]
        public async Task<IActionResult> GetProfile(string email)
        {
            var result = await _userService.GetUserByEmail(email);
            return Ok(result);
        }
        [HttpGet]
        [Route("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {         
            var email = User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (email == null)
                return Unauthorized();
            
            var result = await _userService.GetUserByEmail(email);
            return Ok(result);   
        }
    }
}
