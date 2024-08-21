using System.ComponentModel.DataAnnotations;

namespace CreationsPlatformWebApplication.Models.Manage;

public class DeleteAccountModel
{
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public string Password { get; set; }
}