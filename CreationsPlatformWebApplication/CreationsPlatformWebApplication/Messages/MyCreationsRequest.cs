using CreationsPlatformWebApplication.Enums;
using Microsoft.AspNetCore.Mvc;

namespace CreationsPlatformWebApplication.Messages;

public class MyCreationsRequest
{
    [BindProperty(Name = "genre")] public int? GenreId { get; set; }
    [BindProperty(Name = "title")] public string Title { get; set; }

    [BindProperty(Name = "authorUsername")]
    public string AuthorUsername { get; set; }

    [BindProperty(Name = "page")] public int PageNumber { get; set; } = 1;
    [BindProperty(Name = "pageSize")] public int PageSize { get; set; } = 10;
    [BindProperty(Name = "sortOrder")] public SortState SortOrder { get; set; } = SortState.IdAsc;

    [BindProperty(Name = "publishedBefore")]
    public DateTime? PublishedBefore { get; set; }

    [BindProperty(Name = "publishedAfter")]
    public DateTime? PublishedAfter { get; set; }
}