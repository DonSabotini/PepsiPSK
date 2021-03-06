using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PepsiPSK.Models.User;
using PepsiPSK.Services.Users;

namespace PepsiPSK.Controllers
{
    [Authorize(Roles = "User, Admin")]
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var loginResult = await _userService.Login(loginDto);

            if (loginResult == null || !loginResult.IsSuccessful)
            {
                return NotFound(loginResult);
            }

            return Ok(loginResult);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationDto registrationDto)
        {
            var registrationResult = await _userService.Register(registrationDto);

            if (!registrationResult.IsSuccessful)
            {
                return BadRequest(registrationResult);
            }

            return Ok(registrationResult);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetUsers();
            return Ok(users);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var getResult = await _userService.GetUserById(id);

            if (getResult == null)
            {
                return NotFound();
            }

            return Ok(getResult);
        }

        [HttpPut("{id}/change-password")]
        public async Task<IActionResult> ChangePassword(string id, ChangePasswordDto changePasswordDto)
        {
            var serviceResponse = await _userService.ChangePassword(id, changePasswordDto);

            if (serviceResponse.StatusCode == 500 && serviceResponse.IsOptimisticLocking)
            {
                return StatusCode(serviceResponse.StatusCode, serviceResponse);
            }

            if (serviceResponse.StatusCode == 404)
            {
                return StatusCode(serviceResponse.StatusCode, serviceResponse);
            }

            if (serviceResponse.StatusCode == 401)
            {
                return StatusCode(serviceResponse.StatusCode, serviceResponse);
            }

            return StatusCode(serviceResponse.StatusCode, serviceResponse);
        }

        [HttpPut("{id}/update-details")]
        public async Task<IActionResult> UpdateDetails(string id, UpdateUserDetailsDto updateUserDetailsDto)
        {
            var serviceResponse = await _userService.UpdateUserDetails(id, updateUserDetailsDto);

            if (serviceResponse.StatusCode == 500 && serviceResponse.IsOptimisticLocking)
            {
                return StatusCode(serviceResponse.StatusCode, serviceResponse);
            }

            if (serviceResponse.StatusCode == 404)
            {
                return StatusCode(serviceResponse.StatusCode, serviceResponse);
            }

            return StatusCode(serviceResponse.StatusCode, serviceResponse);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var deleteResult = await _userService.DeleteUser(id);

            if (deleteResult == null)
            {
                return NotFound();
            }

            if (!deleteResult.IsSuccessful)
            {
                return Unauthorized(deleteResult);
            }

            return Ok(deleteResult);
        }
    }
}
