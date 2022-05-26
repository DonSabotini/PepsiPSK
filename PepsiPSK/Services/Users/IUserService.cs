using PepsiPSK.Models.User;
using PepsiPSK.Responses.Authentication;
using PepsiPSK.Responses.Service;

namespace PepsiPSK.Services.Users
{
    public interface IUserService
    {
        Task<AuthenticationResponse?> Login(LoginDto loginDto);
        Task<AuthenticationResponse> Register(RegistrationDto registrationDto);
        Task<List<UserInfoDto>> GetUsers();
        Task<AuthenticationResponse?> GetUserById(string id);
        Task<ServiceResponse<AuthenticationResponse?>> ChangePassword(string id, ChangePasswordDto changePasswordDto);
        Task<ServiceResponse<AuthenticationResponse?>> UpdateUserDetails(string id, UpdateUserDetailsDto updateUserDetailsDto);
        Task<AuthenticationResponse?> DeleteUser(string id);
    }
}
