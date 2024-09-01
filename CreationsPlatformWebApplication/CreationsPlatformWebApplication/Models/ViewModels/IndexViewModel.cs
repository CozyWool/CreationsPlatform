using CreationsPlatformWebApplication.Models.Creation;
using CreationsPlatformWebApplication.Models.ViewModels.FilterSortPaging;

namespace CreationsPlatformWebApplication.Models.ViewModels;

public class IndexViewModel
{
    public IEnumerable<CreationModel> Creations { get; set; }
    public PageViewModel PageViewModel { get; set; }
    public FilterViewModel FilterViewModel { get; set; }
    public SortViewModel SortViewModel { get; set; }
}