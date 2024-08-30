using System.ComponentModel.DataAnnotations;

namespace CreationsPlatformWebApplication.Models.Creation;

public class GenreModel
{
    [Display(Name = "Id")] public int Id { get; set; }
    [Display(Name = "Название")] public string Name { get; set; }
}