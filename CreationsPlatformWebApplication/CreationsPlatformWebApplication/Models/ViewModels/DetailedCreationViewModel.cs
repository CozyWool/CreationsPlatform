using CreationsPlatformWebApplication.Models.Creation;
using CreationsPlatformWebApplication.Models.ViewModels.FilterSortPaging;

namespace CreationsPlatformWebApplication.Models.ViewModels;

public class DetailedCreationViewModel
{
    public CreationModel Creation { get; set; }
    public CommentModel Comment { get; set; }
    public PageViewModel PageViewModel { get; set; }
}