using PepsiPSK.Models.User;
using PepsiPSK.Responses.Authentication;

namespace PepsiPSK.Services.Users
{
    public interface IUserService
    {
        Task<AuthenticationResponse?> Login(LoginDto loginDto);
        Task<AuthenticationResponse> Register(RegistrationDto registrationDto);
        Task<List<UserInfoDto>> GetUsers();
        Task<AuthenticationResponse?> GetUserById(string id);
        Task<AuthenticationResponse?> ChangePassword(string id, ChangePasswordDto changePasswordDto);
        Task<AuthenticationResponse?> UpdateUserDetails(string id, UpdateUserDetailsDto updateUserDetailsDto);
        Task<AuthenticationResponse?> DeleteUser(string id);
    }
}
