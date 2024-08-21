using System.ComponentModel.DataAnnotations;

namespace CreationsPlatformWebApplication.Models.Manage;

public class EmailModel
{
    public StatusMessageModel StatusMessage { get; set; } = new();
    [EmailAddress]
    [Display(Name = "Электронная почта")]
    public string Email { get; set; }
    [EmailAddress]
    [Display(Name = "Новая электронная почта")]
    public string NewEmail { get; set; }
}