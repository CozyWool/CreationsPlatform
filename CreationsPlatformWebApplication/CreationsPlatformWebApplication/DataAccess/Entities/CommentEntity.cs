namespace CreationsPlatformWebApplication.DataAccess.Entities;

public class CommentEntity
{
    public int Id { get; set; }

    public Guid UserId { get; set; }
    public int CreationId { get; set; }

    public string Content { get; set; } = null!;
    public DateTime PublicationDate { get; set; }


    // public virtual CreationEntity Creation { get; set; } = null!;
    public UserEntity User { get; set; } = null!;
}
