using Microsoft.AspNetCore.Mvc;
using PepsiPSK.Models.User;
using PepsiPSK.Services.Users;

namespace PepsiPSK.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var loginResult = await _userService.Login(loginDto);

            if(loginResult == null)
            {
                return NotFound();
            }

            if(!loginResult.IsSuccessful)
            {
                return BadRequest(loginResult);
            }

            return Ok(loginResult);
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationDto registrationDto)
        {
            var registrationResult = await _userService.Register(registrationDto);

            if (!registrationResult.IsSuccessful)
            {
                return Conflict(registrationResult);
            }

            return Ok(registrationResult);
        }
    }
}
