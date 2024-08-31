using System.ComponentModel.DataAnnotations;

namespace CreationsPlatformWebApplication.Models.Creation;

public class CommentModel
{
    [Display(Name = "Id")] public int Id { get; set; }
    [Display(Name = "Id пользователя")] public Guid UserId { get; set; }
    [Display(Name = "Ник")] public string Username { get; set; }

    [Display(Name = "Название произведения")]
    public string CreationTitle { get; set; }

    [Display(Name = "Id произведения")] public int CreationId { get; set; }

    [Display(Name = "Оставьте комментарий")]
    public string Content { get; set; }

    [Display(Name = "Дата публикации")] public DateTime PublicationDate { get; set; }
}