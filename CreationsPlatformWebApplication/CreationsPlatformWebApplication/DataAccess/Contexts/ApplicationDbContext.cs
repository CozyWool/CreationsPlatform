using CreationsPlatformWebApplication.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace CreationsPlatformWebApplication.DataAccess.Contexts;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public virtual DbSet<CommentEntity> Comments { get; set; }
    public virtual DbSet<CreationEntity> Creations { get; set; }
    public virtual DbSet<GenreEntity> Genres { get; set; }
    public virtual DbSet<UserEntity> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CommentEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Comments_pkey");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.CreationId).HasColumnName("creation_id");
            entity.Property(e => e.PublicationDate).HasColumnName("publication_date");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User)
                .WithMany()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .IsRequired();
        });

        modelBuilder.Entity<CreationEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Creations_pkey");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.Content)
                .HasColumnType("text")
                .HasColumnName("content");
            entity.Property(e => e.AuthorId).HasColumnName("author_id");
            entity.Property(e => e.PublicationDate).HasColumnName("publication_date");
            entity.Property(e => e.TotalRating)
                .HasDefaultValue(0)
                .HasColumnName("total_rating");
            entity.Property(e => e.RatingCount)
                .HasDefaultValue(0)
                .HasColumnName("rating_count");
            entity.Property(e => e.CommentCount)
                .HasDefaultValue(0)
                .HasColumnName("comment_count");

            entity.HasOne(d => d.Author)
                .WithMany()
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("author_id_fk");

            entity.HasMany(d => d.Genres)
                .WithMany();

            entity.HasMany(d => d.Comments)
                .WithOne()
                .HasForeignKey(d => d.CreationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .IsRequired();
        });

        modelBuilder.Entity<UserEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Users_pkey");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreatedDate).HasColumnName("created_date");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("is_deleted");
            entity.Property(e => e.Username)
                .HasMaxLength(75)
                .HasColumnName("username");
            entity.Property(e => e.PasswordHash).HasColumnName("password_hash");
        });

        modelBuilder.Entity<GenreEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Genres_pkey");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });
    }
}