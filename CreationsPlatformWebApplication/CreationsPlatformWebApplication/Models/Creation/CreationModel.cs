using System.ComponentModel.DataAnnotations;

namespace CreationsPlatformWebApplication.Models.Creation;

public class CreationModel
{
    [Display(Name = "Id")] public int Id { get; set; }

    [Display(Name = "Название")]
    [Required(ErrorMessage = "Укажите название произведения")]
    public string Title { get; set; }

    [Display(Name = "Жанры")] public List<GenreModel> Genres { get; set; } = [];
    [Display(Name = "Комменатрии")] public List<CommentModel> Comments { get; set; } = [];

    [Display(Name = "Кол-во комментариев")]
    public int CommentCount => Comments.Count;
    [Display(Name = "Дата публикации")] public DateTime PublicationDate { get; set; }

    [Display(Name = "Содержимое")]
    [Required(ErrorMessage = "Произведение не может быть пустым")]
    public string Content { get; set; }

    [Display(Name = "Оценка")] public int TotalRating { get; set; }
    [Display(Name = "Кол-во оценок")] public int RatingCount { get; set; }


    [Display(Name = "Автор")] public AuthorModel Author { get; set; }
}