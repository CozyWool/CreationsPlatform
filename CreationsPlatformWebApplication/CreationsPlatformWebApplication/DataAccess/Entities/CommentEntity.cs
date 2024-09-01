namespace CreationsPlatformWebApplication.DataAccess.Entities;

public class CommentEntity
{
    public int Id { get; set; }

    public Guid UserId { get; set; }
    public int CreationId { get; set; }
    public int Rating { get; set; }

    public string Content { get; set; } = null!;
    public DateTime PublicationDate { get; set; }
    public UserEntity User { get; set; } = null!;
}
