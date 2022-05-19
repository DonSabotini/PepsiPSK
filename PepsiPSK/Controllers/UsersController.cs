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
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var loginResult = await _userService.Login(loginDto);

            if (loginResult == null)
            {
                return NotFound();
            }

            if(!loginResult.IsSuccessful)
            {
                return BadRequest(loginResult);
            }

            return Ok(loginResult);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
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
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var getResult = await _userService.GetUserById(id);

            if (getResult == null)
            {
                return NotFound();
            }

            return Ok(getResult);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(UpdateUserDto updateUserDto)
        {
            var putResult = await _userService.UpdateUser(updateUserDto);

            if (putResult == null)
            {
                return NotFound();
            }

            if (!putResult.IsSuccessful)
            {
                return Unauthorized(putResult);
            }

            return Ok(putResult);
        }

        [HttpDelete]
        [Route("{id}")]
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
