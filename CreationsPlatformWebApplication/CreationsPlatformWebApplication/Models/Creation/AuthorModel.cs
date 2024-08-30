using System.ComponentModel.DataAnnotations;

namespace CreationsPlatformWebApplication.Models.Creation;

public class AuthorModel
{
    [Display(Name = "Id автора")] public Guid Id { get; set; }

    [Display(Name = "Ник автора")]
    public string Username { get; set; }
}