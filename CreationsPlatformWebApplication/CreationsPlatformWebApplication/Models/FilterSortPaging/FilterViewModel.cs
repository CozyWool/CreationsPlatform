using CreationsPlatformWebApplication.Models.Creation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CreationsPlatformWebApplication.Models.FilterSortPaging;

public class FilterViewModel
{
    public FilterViewModel(IList<GenreModel> genres, int? genre, string title, string authorUsername,
        DateTime publishedAfter, DateTime publishedBefore)
    {
        genres.Insert(0, new GenreModel {Name = "Все", Id = -1});
        Genres = new SelectList(genres, "Id", "Name", genre);
        SelectedGenre = genre;
        SelectedTitle = title;
        SelectedAuthorUsername = authorUsername;
        PublishedAfter = publishedAfter;
        PublishedBefore = publishedBefore;
    }


    public SelectList Genres { get; private set; }
    public int? SelectedGenre { get; private set; }
    public string SelectedTitle { get; private set; }
    public string SelectedAuthorUsername { get; private set; }
    public DateTime PublishedAfter { get; private set; }
    public DateTime PublishedBefore { get; private set; }
}