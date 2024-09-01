using System.ComponentModel.DataAnnotations;

namespace CreationsPlatformWebApplication.Models.ViewModels.FilterSortPaging;

public class PageViewModel(int count, int pageNumber, int pageSize)
{
    [Display(Name = "Страница")] public int PageNumber { get; private set; } = pageNumber;
    public int TotalPages { get; private set; } = (int) Math.Ceiling(count / (double) pageSize);


    [Display(Name = "Назад")] public bool HasPreviousPage => PageNumber > 1;
    [Display(Name = "Вперед")] public bool HasNextPage => PageNumber < TotalPages;
    [Display(Name = "Размер страницы")] public int PageSize { get; set; } = pageSize;
}