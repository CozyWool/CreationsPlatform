namespace CreationsPlatformWebApplication.DataAccess.Entities;

public class CreationEntity
{
    public int Id { get; set; }

    public List<string> Genre { get; set; } = null!;

    public DateTime PublicationDate { get; set; }

    public int Rating { get; set; }

    public int RatingCount { get; set; }

    public Guid AuthorId { get; set; }

    public virtual UserEntity Author { get; set; } = null!;

    public virtual ICollection<CommentEntity> Comments { get; set; } = new List<CommentEntity>();
}
