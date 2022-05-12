using PepsiPSK.Authentication;
using PepsiPSK.Entities;
using PepsiPSK.Models.User;

namespace PepsiPSK.Services.Users
{
    public interface IUserService
    {
        Task<AuthenticationResponse?> Login(LoginDto loginDto);
        Task<AuthenticationResponse> Register(RegistrationDto registrationDto);
        Task<User> RetrieveCurrentUser();
    }
}
