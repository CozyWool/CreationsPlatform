using CreationsPlatformWebApplication.Enums;

namespace CreationsPlatformWebApplication.Models.FilterSortPaging;

public class SortViewModel(SortState sortOrder)
{
    public SortState IdSort { get; private set; } = sortOrder == SortState.IdAsc ? SortState.IdDesc : SortState.IdAsc;
    public SortState TitleSort { get; private set; } = sortOrder == SortState.TitleAsc ? SortState.TitleDesc : SortState.TitleAsc;
    public SortState AuthorUsernameSort { get; private set; } = sortOrder == SortState.AuthorUsernameAsc
        ? SortState.AuthorUsernameDesc
        : SortState.AuthorUsernameAsc;

    public SortState PublicationDateSort { get; private set; } = sortOrder == SortState.PublicationDateAsc
        ? SortState.PublicationDateDesc
        : SortState.PublicationDateAsc;

    public SortState RatingSort { get; private set; } = sortOrder == SortState.RatingAsc ? SortState.RatingDesc : SortState.RatingAsc;
    public SortState CommentSort { get; private set; } = sortOrder == SortState.CommentAsc ? SortState.CommentDesc : SortState.CommentAsc;
    public SortState Current { get; private set; } = sortOrder;
}