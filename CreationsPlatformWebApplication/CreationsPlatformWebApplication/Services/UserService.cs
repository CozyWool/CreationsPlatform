using System.Security.Claims;
using AutoMapper;
using CreationsPlatformWebApplication.DataAccess.Entities;
using CreationsPlatformWebApplication.DataAccess.Repositories;
using CreationsPlatformWebApplication.Enums;
using CreationsPlatformWebApplication.Helpers;
using CreationsPlatformWebApplication.Models.Creation;
using CreationsPlatformWebApplication.Models.Manage;
using CreationsPlatformWebApplication.Models.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace CreationsPlatformWebApplication.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper)
    {
        _userRepository = userRepository;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
        if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated) return;
        
        var user = _userRepository.GetByUsername(_httpContextAccessor.HttpContext.User.Identity.Name).Result;
        var rememberMe = _httpContextAccessor.HttpContext
            .User.HasClaim("RememberMe", true.ToString());
        Authenticate(user, rememberMe);
    }

    public async Task<UserServiceStatusCodes> Login(LoginModel model)
    {
        var user = await _userRepository.GetByEmailOrUsername(model.UsernameOrEmail, model.UsernameOrEmail);
        if (user == null)
            return UserServiceStatusCodes.NotFound;
        if (!SecurityHelper.PasswordMatch(model.Password, user.Id.ToString(), user.PasswordHash))
            return UserServiceStatusCodes.NotValid;
        if (user.IsDeleted)
            return UserServiceStatusCodes.AccountDeleted;

        await Authenticate(user, model.RememberMe);
        return UserServiceStatusCodes.OK;
    }

    public async Task<bool> Logout()
    {
        await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return true;
    }

    public async Task<UserServiceStatusCodes> Register(RegisterModel model)
    {
        var user = await _userRepository.GetByEmailOrUsername(model.Email, model.Username);
        if (user is {IsDeleted: true}) return UserServiceStatusCodes.AccountDeleted;
        if (user != null) return UserServiceStatusCodes.AlreadyExist;

        var id = Guid.NewGuid();
        var hash = SecurityHelper.GenerateSaltedHash(model.Password, id.ToString());
        var userEntity = new UserEntity
        {
            Id = id,
            Email = model.Email,
            Username = model.Username,
            PasswordHash = hash,
            CreatedDate = DateTime.UtcNow
        };
        await _userRepository.Create(userEntity);

        await Authenticate(userEntity, false);
        return UserServiceStatusCodes.OK;
    }

    public async Task<UserServiceStatusCodes> UpdateProfile(ProfileModel model)
    {
        if (model.OldUsername == model.Username) return UserServiceStatusCodes.OK;

        var user = await _userRepository.GetByUsername(model.OldUsername);
        if (user == null) return UserServiceStatusCodes.NotFound;
        var isNewUsernameBusy = await _userRepository.GetByUsername(model.Username) != null;
        if (isNewUsernameBusy) return UserServiceStatusCodes.AlreadyExist;

        user.Username = model.Username;
        await _userRepository.Update(user);
        await Authenticate(user, _httpContextAccessor.HttpContext.User.HasClaim("RememberMe", true.ToString()));
        return UserServiceStatusCodes.OK;
    }

    public async Task<UserServiceStatusCodes> UpdateEmail(EmailModel model)
    {
        if (model.Email == model.NewEmail) return UserServiceStatusCodes.OK;

        var user = await _userRepository.GetByEmail(model.Email);
        if (user == null) return UserServiceStatusCodes.NotFound;
        var isNewEmailBusy = await _userRepository.GetByEmail(model.NewEmail) != null;
        if (isNewEmailBusy) return UserServiceStatusCodes.AlreadyExist;

        user.Email = model.NewEmail;
        await _userRepository.Update(user);
        await Authenticate(user, _httpContextAccessor.HttpContext.User.HasClaim("RememberMe", true.ToString()));
        return UserServiceStatusCodes.OK;
    }

    public async Task<UserServiceStatusCodes> UpdatePassword(PasswordModel model)
    {
        if (model.CurrentPassword == model.NewPassword) return UserServiceStatusCodes.AlreadyExist;

        var user = await _userRepository.GetByUsername(_httpContextAccessor.HttpContext.User.Identity.Name);
        if (user == null) return UserServiceStatusCodes.NotFound;

        if (!SecurityHelper.PasswordMatch(model.CurrentPassword, user.Id.ToString(), user.PasswordHash))
            return UserServiceStatusCodes.NotValid;

        user.PasswordHash = SecurityHelper.GenerateSaltedHash(model.NewPassword, user.Id.ToString());
        await _userRepository.Update(user);
        //TODO: Надо бы как-нибудь разлогинть все существующие сессии пользователя, но я без понятия как
        return UserServiceStatusCodes.OK;
    }

    public async Task<UserServiceStatusCodes> VerifyPassword(string usernameOrLogin, string password)
    {
        var user = await _userRepository.GetByEmailOrUsername(usernameOrLogin, usernameOrLogin);
        if (user == null) return UserServiceStatusCodes.NotFound;
        return SecurityHelper.PasswordMatch(password, user.Id.ToString(), user.PasswordHash)
            ? UserServiceStatusCodes.OK
            : UserServiceStatusCodes.NotValid;
    }

    public async Task<UserServiceStatusCodes> DeleteUser(string name)
    {
        if (!await _userRepository.Delete(name))
            return UserServiceStatusCodes.NotFound;
        return UserServiceStatusCodes.OK;
    }

    public async Task<AuthorModel?> GetAuthorById(Guid id)
    {
        var user = await _userRepository.GetById(id);
        return _mapper.Map<AuthorModel>(user) ?? null;
    }

    private async Task Authenticate(UserEntity user, bool rememberMe)
    {
        if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
        {
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Email, user.Email),
            new("CreatedDate", user.CreatedDate.ToShortDateString()),
            new("UserId", user.Id.ToString())
        };
        if (rememberMe)
        {
            claims.Add(new Claim("RememberMe", rememberMe.ToString()));
        }

        var id = new ClaimsIdentity(claims,
            "ApplicationCookie",
            ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType);
        await _httpContextAccessor.HttpContext
            .SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(id),
                new AuthenticationProperties {IsPersistent = rememberMe});
    }
    
}