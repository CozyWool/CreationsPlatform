using CreationsPlatformWebApplication.Models.Creation;

namespace CreationsPlatformWebApplication.Models.FilterSortPaging;

public class IndexViewModel
{
    public IEnumerable<CreationModel> Creations { get; set; }
    public PageViewModel PageViewModel { get; set; }
    public FilterViewModel FilterViewModel { get; set; }
    public SortViewModel SortViewModel { get; set; }
}