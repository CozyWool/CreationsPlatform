using System.Globalization;
using System.Security.Claims;
using CreationsPlatformWebApplication.Enums;
using CreationsPlatformWebApplication.Models;
using CreationsPlatformWebApplication.Models.Manage;
using CreationsPlatformWebApplication.Models.User;
using CreationsPlatformWebApplication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CreationsPlatformWebApplication.Controllers;

[Route("[controller]/[action]")]
public class AccountController(IUserService userService) : Controller
{
    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        ViewBag.Title = "Вход";
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginModel model, string? returnUrl = null)
    {
        if (!ModelState.IsValid) return View(model);

        var statusCode = await userService.Login(model);
        switch (statusCode)
        {
            case UserServiceStatusCodes.OK:
                if (!string.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                return RedirectToAction("Index", "CreationsPlatform");
            case UserServiceStatusCodes.AccountDeleted:
                ModelState.AddModelError("", "Пользователь удалён");
                break;
            case UserServiceStatusCodes.NotFound:
                ModelState.AddModelError("", "Пользователь не найден");
                break;
            case UserServiceStatusCodes.NotValid:
                ModelState.AddModelError("", "Неверный пароль или логин");
                break;
        }
        ViewData["Title"] = "Вход";
        return View(model);
    }


    [HttpGet]
    public IActionResult Register(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        ViewData["Title"] = "Регистрация";
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterModel model, string? returnUrl = null)
    {
        if (!ModelState.IsValid) return View(model);

        var statusCode = await userService.Register(model);
        switch (statusCode)
        {
            case UserServiceStatusCodes.OK:
                if (!string.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                return RedirectToAction("Index", "CreationsPlatform");
            case UserServiceStatusCodes.AccountDeleted:
                ModelState.AddModelError("", "Пользователь удалён");
                break;
            case UserServiceStatusCodes.AlreadyExist:
                ModelState.AddModelError("", "Пользователь уже существует");
                break;
        }
        ViewData["Title"] = "Регистрация";
        return View(model);
    }

    public async Task<IActionResult> Logout()
    {
        ViewData["Title"] = "Выход из аккаунта";
        
        if (await userService.Logout())
            return RedirectToAction("Login", "Account");
        return RedirectToAction("Index", "CreationsPlatform");
    }

    public async Task<IActionResult> AccessDenied(string? returnUrl = null)
    {
        ViewData["ErrorMessage"] = "Доступ к странице запрещён";
        ViewData["Title"] = "Доступ запрещён";
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Profile(StatusMessageModel statusMessage = null)
    {
        ViewData["ActivePage"] = nameof(Profile);
        ViewData["Title"] = "Профиль";

        var username = User.Identity.Name;
        var model = new ProfileModel
        {
            OldUsername = username,
            Username = username,
            CreatedDate = DateTime.Parse(User.Claims.FirstOrDefault(claim => claim.Type == "CreatedDate").Value,
                CultureInfo.CurrentCulture),
            StatusMessage = statusMessage
        };
        return View($"Manage/{nameof(Profile)}", model);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Profile(ProfileModel model)
    {
        var statusCode = await userService.UpdateProfile(model);
        // обожаю этот новый сахар
        model.StatusMessage = statusCode switch
        {
            UserServiceStatusCodes.OK => new StatusMessageModel("Профиль успешно обновлён"),
            UserServiceStatusCodes.AlreadyExist => new StatusMessageModel("Имя пользователя уже используется", true),
            UserServiceStatusCodes.NotFound => new StatusMessageModel("Пользователь не найден", true),
            _ => model.StatusMessage
        };

        return RedirectToAction("Profile", model.StatusMessage);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Email(StatusMessageModel statusMessage = null)
    {
        ViewData["ActivePage"] = nameof(Email);
        ViewData["Title"] = "Электронная почта";

        var email = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email).Value;
        var model = new EmailModel
        {
            Email = email,
            StatusMessage = statusMessage
        };
        return View($"Manage/{nameof(Email)}", model);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Email(EmailModel model)
    {
        var statusCode = await userService.UpdateEmail(model);
        // обожаю этот новый сахар
        model.StatusMessage = statusCode switch
        {
            UserServiceStatusCodes.OK => new StatusMessageModel("Эл. почта успешно обновлена"),
            UserServiceStatusCodes.AlreadyExist => new StatusMessageModel("Эл. почта уже используется", true),
            UserServiceStatusCodes.NotFound => new StatusMessageModel("Пользователь не найден", true),
            _ => model.StatusMessage
        };

        return RedirectToAction("Email", model.StatusMessage);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Password(StatusMessageModel statusMessage = null)
    {
        ViewData["ActivePage"] = nameof(Password);
        ViewData["Title"] = "Изменение пароля";

        var model = new PasswordModel
        {
            StatusMessage = statusMessage,
        };
        return View($"Manage/{nameof(Password)}", model);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Password(PasswordModel model)
    {
        ViewData["ActivePage"] = nameof(Password);

        var statusCode = await userService.UpdatePassword(model);
        // обожаю этот новый сахар
        model.StatusMessage = statusCode switch
        {
            UserServiceStatusCodes.OK => new StatusMessageModel("Пароль успешно обновлён"),
            UserServiceStatusCodes.NotFound => new StatusMessageModel("Пользователь не найден", true),
            UserServiceStatusCodes.NotValid => new StatusMessageModel("Текущий пароль введён неверно", true),
            UserServiceStatusCodes.AlreadyExist => new StatusMessageModel("Новый пароль должен отличаться от текущего",
                true),
            _ => model.StatusMessage
        };

        return RedirectToAction("Password", model.StatusMessage);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> DeleteAccount()
    {
        ViewData["ActivePage"] = nameof(DeleteAccount);
        ViewData["Title"] = "Удаление аккаунта";

        return View($"Manage/{nameof(DeleteAccount)}");
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> DeleteAccount(DeleteAccountModel model)
    {
        ViewData["ActivePage"] = nameof(DeleteAccount);
        var isPasswordValid = await userService.VerifyPassword(User.Identity.Name, model.Password) ==
                              UserServiceStatusCodes.OK;

        if (!isPasswordValid)
        {
            ModelState.AddModelError("", "Неверный пароль");
            return View($"Manage/{nameof(DeleteAccount)}");
        }

        await userService.DeleteUser(User.Identity.Name);
        await userService.Logout();
        return RedirectToAction("Register");
    }
}