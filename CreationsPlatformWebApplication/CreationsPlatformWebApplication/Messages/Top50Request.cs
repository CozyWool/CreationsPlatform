using CreationsPlatformWebApplication.Enums;
using Microsoft.AspNetCore.Mvc;

namespace CreationsPlatformWebApplication.Messages;

public class Top50Request
{
    [BindProperty(Name = "authorUsername")]
    public string AuthorUsername { get; set; }

    [BindProperty(Name = "page")] public int PageNumber { get; set; } = 1;
    [BindProperty(Name = "pageSize")] public int PageSize { get; set; } = 10;
    [BindProperty(Name = "sortOrder")] public SortState SortOrder { get; set; } = SortState.IdAsc;
}