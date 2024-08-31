using System.ComponentModel.DataAnnotations;

namespace CreationsPlatformWebApplication.Models.Creation;

public class AuthorModel
{
    [Display(Name = "Id")] public Guid Id { get; set; }

    [Display(Name = "Ник")]
    public string Username { get; set; }
}