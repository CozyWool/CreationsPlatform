using CreationsPlatformWebApplication.Models;
using CreationsPlatformWebApplication.Models.Manage;
using CreationsPlatformWebApplication.Services.Enums;

namespace CreationsPlatformWebApplication.Services;

public interface IUserService
{
    Task<UserServiceStatusCodes> Login(LoginModel model);
    Task<bool> Logout();
    Task<UserServiceStatusCodes> Register(RegisterModel model);
    Task<UserServiceStatusCodes> UpdateProfile(ProfileModel model);
    Task<UserServiceStatusCodes> UpdateEmail(EmailModel model);
    Task<UserServiceStatusCodes> UpdatePassword(PasswordModel model);
    Task<UserServiceStatusCodes> VerifyPassword(string usernameOrLogin, string password);
    Task<UserServiceStatusCodes> DeleteUser(string name);
    Task<AuthorModel?> GetAuthorById(Guid id);
}
