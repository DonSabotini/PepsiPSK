using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using PepsiPSK.Constants;
using PepsiPSK.Entities;
using PepsiPSK.Models.User;
using PepsiPSK.Responses.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PepsiPSK.Services.Users
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        public UserService(UserManager<User> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<AuthenticationResponse?> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.LoginUsername);

            var respone = new AuthenticationResponse();

            if (user == null) return null;

            bool isCorrectPassword = await _userManager.CheckPasswordAsync(user, loginDto.LoginPassword);

            if (!isCorrectPassword)
            {
                respone.IsSuccessful = false;
                respone.Message = "Wrong password!";
                respone.Content = null;
                return respone;
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                audience: _configuration["JWT:ValidAudience"],
                issuer: _configuration["JWT:ValidIssuer"],
                expires: DateTime.Now.AddHours(12),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            var writtenToken = new JwtSecurityTokenHandler().WriteToken(token);

            respone.IsSuccessful = true;
            respone.Message = "Logged in successfully!";
            respone.Content = writtenToken;
            return respone;
        }

        public async Task<AuthenticationResponse> Register(RegistrationDto registrationDto)
        {
            var respone = new AuthenticationResponse();

            var existingUser = await _userManager.FindByNameAsync(registrationDto.RegistrationUsername);

            if (existingUser != null)
            {
                respone.IsSuccessful = false;
                respone.Message = "User with such username already exists!";
                respone.Content = null;
                return respone;
            }

            User user = new()
            {
                UserName = registrationDto.RegistrationUsername,
                Email = registrationDto.RegistrationEmail,
                FirstName = registrationDto.FirstName,
                LastName = registrationDto.LastName,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            var identityResult = await _userManager.CreateAsync(user, registrationDto.RegistrationPassword);

            if (!identityResult.Succeeded)
            {
                respone.IsSuccessful = false;
                respone.Message = "Failed to register!";
                respone.Content = identityResult;
                return respone;
            }

            await _userManager.AddToRoleAsync(user, RoleList.User);

            respone.IsSuccessful = true;
            respone.Message = "Registered successfully!";
            respone.Content = null;
            return respone;
        }
    }
}
