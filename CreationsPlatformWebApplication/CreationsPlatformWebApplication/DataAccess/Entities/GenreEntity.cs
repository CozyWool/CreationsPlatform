namespace CreationsPlatformWebApplication.DataAccess.Entities;

public class GenreEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
    // public virtual ICollection<CreationEntity> Creations { get; set; } = new List<CreationEntity>();
}