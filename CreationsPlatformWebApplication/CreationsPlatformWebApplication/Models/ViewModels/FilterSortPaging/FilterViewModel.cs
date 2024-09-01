using System.ComponentModel.DataAnnotations;
using CreationsPlatformWebApplication.Models.Creation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CreationsPlatformWebApplication.Models.ViewModels.FilterSortPaging;

public class FilterViewModel
{
    public FilterViewModel(IList<GenreModel> genres, int? genre, string title, string authorUsername,
        DateTime? publishedAfter, DateTime? publishedBefore, int? ratingBefore, int? ratingAfter)
    {
        genres.Insert(0, new GenreModel {Name = "Все", Id = -1});
        Genres = new SelectList(genres, "Id", "Name", genre);
        SelectedGenre = genre;
        SelectedTitle = title;
        SelectedAuthorUsername = authorUsername;
        PublishedAfter = publishedAfter;
        PublishedBefore = publishedBefore;
        RatingBefore = ratingBefore ?? 100;
        RatingAfter = ratingAfter ?? 0;
    }
    
    public SelectList Genres { get; private set; }
    [Display(Name = "Жанр")] public int? SelectedGenre { get; private set; }
    [Display(Name = "Название")] public string SelectedTitle { get; private set; }
    [Display(Name = "Ник автора")] public string SelectedAuthorUsername { get; private set; }
    [Display(Name = "Опубликовано после")] public DateTime? PublishedAfter { get; private set; }
    [Display(Name = "Опубликовано до")] public DateTime? PublishedBefore { get; private set; }
    [Display(Name = "Рейтинг до")] public int RatingBefore { get; private set; }
    [Display(Name = "Рейтинг от")] public int RatingAfter { get; private set; }
}