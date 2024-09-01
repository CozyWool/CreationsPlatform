namespace CreationsPlatformWebApplication.DataAccess.Entities;

public class CreationEntity
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime PublicationDate { get; set; }
    public int TotalRating { get; set; }
    public int RatingCount { get; set; }
    public int CommentCount { get; set; }
    public Guid AuthorId { get; set; }
    public virtual UserEntity Author { get; set; } = null!;
    public virtual ICollection<CommentEntity> Comments { get; set; } = new List<CommentEntity>();
    public virtual ICollection<GenreEntity> Genres { get; set; } = new List<GenreEntity>();
}