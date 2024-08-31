using CreationsPlatformWebApplication.Models.Creation;

namespace CreationsPlatformWebApplication.Models.ViewModels;

public class DetailedCreationViewModel
{
    public CreationModel Creation { get; set; }
    public CommentModel Comment { get; set; }
}