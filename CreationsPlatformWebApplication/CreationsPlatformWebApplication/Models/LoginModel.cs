using System.ComponentModel.DataAnnotations;

namespace CreationsPlatformWebApplication.Models;

public class LoginModel
{
    [Required(ErrorMessage = "Укажите Email")]
    [EmailAddress]
    public string Email { get; set; }

    [Required(ErrorMessage = "Укажите пароль")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Display(Name = "Запомнить")] public bool RememberMe { get; set; }
}