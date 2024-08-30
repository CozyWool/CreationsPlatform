using System.ComponentModel.DataAnnotations;

namespace CreationsPlatformWebApplication.Models.Creation;

public class AuthorModel
{
    [Display(Name = "Id автора")] public Guid Id { get; set; }

    [Display(Name = "Имя пользователя автора")]
    public string Username { get; set; }
}