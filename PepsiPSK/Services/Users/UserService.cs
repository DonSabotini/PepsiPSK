using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using PepsiPSK.Constants;
using PepsiPSK.Entities;
using PepsiPSK.Models.User;
using PepsiPSK.Responses.Authentication;
using PepsiPSK.Utils.Authentication;
using Pepsi.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using PepsiPSK.Responses.Service;

namespace PepsiPSK.Services.Users
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ICurrentUserInfoRetriever _currentUserInfoRetriever;
        private readonly IMapper _mapper;

        public UserService(DataContext context, UserManager<User> userManager, IConfiguration configuration, ICurrentUserInfoRetriever currentUserInfoRetriever, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
            _currentUserInfoRetriever = currentUserInfoRetriever;
            _mapper = mapper;
        }

        private string GetCurrentUserId()
        {
            return _currentUserInfoRetriever.RetrieveCurrentUserId();
        }

        private bool AdminCheck()
        {
            return _currentUserInfoRetriever.CheckIfCurrentUserIsAdmin();
        }

        public async Task<AuthenticationResponse?> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            var respone = new AuthenticationResponse();

            if (user == null) return null;

            bool isCorrectPassword = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!isCorrectPassword)
            {
                respone.Message = "Wrong credentials!";
                return respone;
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
                {
                    new Claim("username", user.UserName),
                    new Claim("id", user.Id),
                };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim("role", userRole));
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

            var existingUser = await _userManager.FindByNameAsync(registrationDto.Username);

            if (existingUser != null)
            {
                respone.Message = "User with such username already exists!";
                return respone;
            }

            User user = new()
            {
                UserName = registrationDto.Username,
                Email = registrationDto.Email,
                FirstName = registrationDto.FirstName,
                LastName = registrationDto.LastName,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            var identityResult = await _userManager.CreateAsync(user, registrationDto.Password);

            if (!identityResult.Succeeded)
            {
                respone.Message = "Failed to register!";
                respone.Content = identityResult;
                return respone;
            }

            await _userManager.AddToRoleAsync(user, RoleList.User);

            respone.IsSuccessful = true;
            respone.Message = "Registered successfully!";
            return respone;
        }

        public async Task<List<UserInfoDto>> GetUsers()
        {
            return await _context.Users.Select(user => _mapper.Map<UserInfoDto>(user)).ToListAsync();
        }

        public async Task<AuthenticationResponse?> GetUserById(string id)
        {
            var user = await _context.FindAsync<User>(id);

            if (user == null)
            {
                return null;
            }

            var respone = new AuthenticationResponse
            {
                IsSuccessful = true,
                Message = "User successfully retrieved!",
                Content = _mapper.Map<UserInfoDto>(user)
            };

            return respone;
        }

        public async Task<ServiceResponse<AuthenticationResponse?>> ChangePassword(string id, ChangePasswordDto changePasswordDto)
        {
            var user = await _context.FindAsync<User>(id);
            var serviceResponse = new ServiceResponse<AuthenticationResponse?>();

            if (user == null)
            {
                serviceResponse.Data = null;
                serviceResponse.StatusCode = 404;
                serviceResponse.Message = $"User with ID of {id} was not found!";

                return serviceResponse;
            }

            if (Nullable.Compare(changePasswordDto.LastModified, user.LastModified) != 0)
            {
                serviceResponse.Data = null;
                serviceResponse.StatusCode = 500;
                serviceResponse.IsOptimisticLocking = true;
                serviceResponse.Message = $"User with ID of {id} has already been updated!";

                return serviceResponse;
            }

            var respone = new AuthenticationResponse();
            bool isCorrectPassword = await _userManager.CheckPasswordAsync(user, changePasswordDto.OldPassword);

            if (AdminCheck() || id == GetCurrentUserId())
            {
                if (isCorrectPassword && changePasswordDto.NewPassword.Equals(changePasswordDto.NewPasswordRepeated))
                {
                    user.LastModified = DateTime.UtcNow;
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    await _userManager.ResetPasswordAsync(user, token, changePasswordDto.NewPassword);
                    await _context.SaveChangesAsync();

                    respone.IsSuccessful = true;
                    respone.Message = "Password successfully changed!";

                    serviceResponse.Data = respone;
                    serviceResponse.StatusCode = 200;
                    serviceResponse.IsSuccessful = true;
                    serviceResponse.Message = $"User with ID of {id} was successfully updated!";

                    return serviceResponse;
                }
                else
                {
                    respone.Message = "Failed to change password!";
                    serviceResponse.Data = null;
                    serviceResponse.StatusCode = 400;
                    serviceResponse.Message = $"Password change failed! Please check your entries!";

                    return serviceResponse;
                }
            }

            respone.Message = "You have no rights to perform this operation!";
            serviceResponse.Data = respone;
            serviceResponse.StatusCode = 401;
            serviceResponse.Message = $"Failed to update user with ID of {id}!";

            return serviceResponse;
        }

        public async Task<ServiceResponse<AuthenticationResponse?>> UpdateUserDetails(string id, UpdateUserDetailsDto updateUserDetailsDto)
        {
            var user = await _context.FindAsync<User>(id);
            var serviceResponse = new ServiceResponse<AuthenticationResponse?>();

            if (user == null)
            {
                serviceResponse.Data = null;
                serviceResponse.StatusCode = 404;
                serviceResponse.Message = $"User with ID of {id} was not found!";

                return serviceResponse;
            }

            if (Nullable.Compare(updateUserDetailsDto.LastModified, user.LastModified) != 0)
            {
                serviceResponse.Data = null;
                serviceResponse.StatusCode = 500;
                serviceResponse.IsOptimisticLocking = true;
                serviceResponse.Message = $"User with ID of {id} has already been updated!";

                return serviceResponse;
            }

            var respone = new AuthenticationResponse();

            if (AdminCheck() || id == GetCurrentUserId())
            {
                user.LastModified = DateTime.UtcNow;
                
                if (updateUserDetailsDto.NewUsername != null)
                {
                    user.UserName = updateUserDetailsDto.NewUsername;
                }

                if (updateUserDetailsDto.NewFirstName != null)
                {
                    user.FirstName = updateUserDetailsDto.NewFirstName;
                }

                if (updateUserDetailsDto.NewLastName != null)
                {
                    user.LastName = updateUserDetailsDto.NewLastName;
                }

                await _context.SaveChangesAsync();

                respone.IsSuccessful = true;
                respone.Message = "Details successfully updated!";
                respone.Content = _mapper.Map<UserInfoDto>(user);

                serviceResponse.Data = respone;
                serviceResponse.StatusCode = 200;
                serviceResponse.IsSuccessful = true;
                serviceResponse.Message = $"User with ID of {id} was successfully updated!";

                return serviceResponse;
            }

            respone.Message = "You have no rights to perform this operation!";

            serviceResponse.Data = respone;
            serviceResponse.StatusCode = 401;
            serviceResponse.Message = $"Failed to update user with ID of {id}!";

            return serviceResponse;
        }

        public async Task<AuthenticationResponse?> DeleteUser(string id)
        {
            var user = await _context.FindAsync<User>(id);

            if (user == null)
            {
                return null;
            }

            var respone = new AuthenticationResponse();

            if (AdminCheck() || id == GetCurrentUserId())
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                respone.IsSuccessful = true;
                respone.Message = "User successfully deleted!";
                return respone;
            }

            respone.Message = "You have no rights to perform this operation!";
            return respone;
        }
    }
}
