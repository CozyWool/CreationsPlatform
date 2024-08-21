namespace CreationsPlatformWebApplication.DataAccess.Entities;

public class UserEntity
{
    public Guid Id { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<CommentEntity> Comments { get; set; } = new List<CommentEntity>();

    public virtual ICollection<CreationEntity> Creations { get; set; } = new List<CreationEntity>();
}
