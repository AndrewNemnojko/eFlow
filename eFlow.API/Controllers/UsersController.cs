
using eFlow.API.Contracts.Users;
using eFlow.API.DTOs.Flowers;
using eFlow.API.DTOs.Users;
using eFlow.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace eFlow.API.Controllers
{
    [ApiController]
    [Route("users")]  
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;
        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("accounts")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllAccounts()
        {
            try
            {
                var data = await _userService.GetUsersAsync();
                var response = new UsersResponse<List<UserDTO>>(
                    true, data.Select(x => new UserDTO
                    (x.Name, x.Email, x.Role.ToString())).ToList(),
                    "Users successful loaded");
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("profile/{email}")]
        [Authorize(Roles = "Manager, Admin")]
        public async Task<IActionResult> GetProfile(string email)
        {
            try
            {
                var result = await _userService.GetUserByEmail(email);
                if (result == null) return NotFound();            
                var response = new UsersResponse<UserDTO>(
                    true, new UserDTO
                    (result.Name, result.Email, result.Role.ToString()),
                    "User successful loaded");
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
        [HttpGet]
        [Route("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var email = User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                if (email == null)
                    return Unauthorized();

                var result = await _userService.GetUserByEmail(email);
                if (result == null) return NotFound();
                var response = new UsersResponse<UserDTO>(
                    true, new UserDTO
                    (result.Name, result.Email, result.Role.ToString()),
                    "User successful loaded");
                return Ok(response);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
    }
}
