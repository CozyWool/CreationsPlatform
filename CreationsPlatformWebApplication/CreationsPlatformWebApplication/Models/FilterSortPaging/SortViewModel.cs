using CreationsPlatformWebApplication.Enums;

namespace CreationsPlatformWebApplication.Models.FilterSortPaging;

public class SortViewModel
{
    public SortState IdSort { get; private set; }
    public SortState TitleSort { get; private set; }
    public SortState AuthorUsernameSort { get; private set; }
    public SortState PublicationDateSort { get; private set; }
    public SortState RatingSort { get; private set; }
    public SortState Current { get; private set; }

    public SortViewModel(SortState sortOrder)
    {
        IdSort = sortOrder == SortState.IdAsc ? SortState.IdDesc : SortState.IdAsc;
        TitleSort = sortOrder == SortState.TitleAsc ? SortState.TitleDesc : SortState.TitleAsc;
        AuthorUsernameSort = sortOrder == SortState.AuthorUsernameAsc
            ? SortState.AuthorUsernameDesc
            : SortState.AuthorUsernameAsc;
        PublicationDateSort = sortOrder == SortState.PublicationDateAsc
            ? SortState.PublicationDateDesc
            : SortState.PublicationDateAsc;
        RatingSort = sortOrder == SortState.RatingAsc ? SortState.RatingDesc : SortState.RatingAsc;
        Current = sortOrder;
    }
}