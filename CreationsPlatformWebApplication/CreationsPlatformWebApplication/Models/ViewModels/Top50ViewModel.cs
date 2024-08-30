using CreationsPlatformWebApplication.Models.Creation;
using CreationsPlatformWebApplication.Models.FilterSortPaging;

namespace CreationsPlatformWebApplication.Models.ViewModels;

public class Top50ViewModel
{
    public IEnumerable<CreationModel> Creations { get; set; }
    public PageViewModel PageViewModel { get; set; }
    public SortViewModel SortViewModel { get; set; }
}