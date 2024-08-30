using System.ComponentModel.DataAnnotations;

namespace CreationsPlatformWebApplication.Models.FilterSortPaging;

public class PageViewModel
{
    [Display(Name = "Страница")] public int PageNumber { get; private set; }
    public int TotalPages { get; private set; }

    public PageViewModel(int count, int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalPages = (int) Math.Ceiling(count / (double) pageSize);
    }


    [Display(Name = "Назад")] public bool HasPreviousPage => PageNumber > 1;
    [Display(Name = "Вперед")] public bool HasNextPage => PageNumber < TotalPages;
    [Display(Name = "Размер страницы")] public int PageSize { get; set; } = 10;
}