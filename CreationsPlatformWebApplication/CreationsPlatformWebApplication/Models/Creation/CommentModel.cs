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
    [Required(ErrorMessage = "Комментарий не может быть пустым")]
    public string Content { get; set; }

    [Display(Name = "Оцените произведение")]
    [Range(0, 100,
        ErrorMessage = "Значение {0} должно быть в пределах {1} и {2}.")]
    public int Rating { get; set; }

    [Display(Name = "Дата публикации")] public DateTime PublicationDate { get; set; }
}