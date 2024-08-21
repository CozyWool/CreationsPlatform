using System.ComponentModel.DataAnnotations;

namespace CreationsPlatformWebApplication.Models.Manage;

public class ProfileModel
{
    public StatusMessageModel StatusMessage { get; set; } = new();

    [Display(Name = "Имя пользователя")]
    public string Username { get; set; }
    public string OldUsername { get; set; }
    [Display(Name = "Дата создания")]
    [DataType(DataType.Date)]
    public DateTime CreatedDate { get; set; }
}