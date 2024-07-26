
using eFlow.Application.DTOs;
using eFlow.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace eFlow.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;
        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllAccounts()
        {
            try
            {
                var data = await _userService.GetUsersAsync();
                var response = data.Select(x => new UserDTO
                    (x.Name, x.Email, x.Role.ToString()));                  
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
                var response = new UserDTO
                    (result.Name, result.Email, result.Role.ToString());
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
                var response = new UserDTO
                    (result.Name, result.Email,result.Role.ToString());
                return Ok(response);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
    }
}
