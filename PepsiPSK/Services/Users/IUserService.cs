using PepsiPSK.Models.User;
using PepsiPSK.Responses.Authentication;

namespace PepsiPSK.Services.Users
{
    public interface IUserService
    {
        Task<AuthenticationResponse?> Login(LoginDto loginDto);
        Task<AuthenticationResponse> Register(RegistrationDto registrationDto);
        Task<List<UserInfo>> GetUsers();
        Task<AuthenticationResponse?> GetUserById(string id);
        Task<AuthenticationResponse?> UpdateUser(UpdateUserDto updateUserDto);
        Task<AuthenticationResponse?> DeleteUser(string guid);
    }
}
