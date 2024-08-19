using CreationsPlatformWebApplication.Models;

namespace CreationsPlatformWebApplication.Services;

public interface IUserService
{
    Task<bool> Login(LoginModel model);
    Task<bool> Logout();
    Task<bool> Register(RegisterModel model);
}