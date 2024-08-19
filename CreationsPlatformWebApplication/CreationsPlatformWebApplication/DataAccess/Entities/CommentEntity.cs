namespace CreationsPlatformWebApplication.DataAccess.Entities;

public class CommentEntity
{
    public int Id { get; set; }

    public Guid UserId { get; set; }

    public int CreationId { get; set; }

    public string Content { get; set; } = null!;

    public virtual CreationEntity CreationEntity { get; set; } = null!;

    public virtual UserEntity UserEntity { get; set; } = null!;
}
