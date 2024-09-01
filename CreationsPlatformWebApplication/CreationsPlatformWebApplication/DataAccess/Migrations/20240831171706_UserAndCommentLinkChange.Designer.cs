﻿// <auto-generated />
using System;
using CreationsPlatformWebApplication.DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CreationsPlatformWebApplication.DataAccess.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240831171706_UserAndCommentLinkChange")]
    partial class UserAndCommentLinkChange
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CreationEntityGenreEntity", b =>
                {
                    b.Property<int>("CreationEntityId")
                        .HasColumnType("integer");

                    b.Property<int>("GenresId")
                        .HasColumnType("integer");

                    b.HasKey("CreationEntityId", "GenresId");

                    b.HasIndex("GenresId");

                    b.ToTable("CreationEntityGenreEntity");
                });

            modelBuilder.Entity("CreationsPlatformWebApplication.DataAccess.Entities.CommentEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("content");

                    b.Property<int>("CreationId")
                        .HasColumnType("integer")
                        .HasColumnName("creation_id");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("Comments_pkey");

                    b.HasIndex("CreationId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("CreationsPlatformWebApplication.DataAccess.Entities.CreationEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uuid")
                        .HasColumnName("author_id");

                    b.Property<int>("CommentCount")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0)
                        .HasColumnName("comment_count");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("content");

                    b.Property<DateTime>("PublicationDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("publication_date");

                    b.Property<int>("Rating")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0)
                        .HasColumnName("rating");

                    b.Property<int>("RatingCount")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0)
                        .HasColumnName("rating_count");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.HasKey("Id")
                        .HasName("Creations_pkey");

                    b.HasIndex("AuthorId");

                    b.ToTable("Creations");
                });

            modelBuilder.Entity("CreationsPlatformWebApplication.DataAccess.Entities.GenreEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("Genres_pkey");

                    b.ToTable("Genres");
                });

            modelBuilder.Entity("CreationsPlatformWebApplication.DataAccess.Entities.UserEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_date");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("email");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("is_deleted");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("password_hash");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(75)
                        .HasColumnType("character varying(75)")
                        .HasColumnName("username");

                    b.HasKey("Id")
                        .HasName("Users_pkey");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("CreationEntityGenreEntity", b =>
                {
                    b.HasOne("CreationsPlatformWebApplication.DataAccess.Entities.CreationEntity", null)
                        .WithMany()
                        .HasForeignKey("CreationEntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CreationsPlatformWebApplication.DataAccess.Entities.GenreEntity", null)
                        .WithMany()
                        .HasForeignKey("GenresId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CreationsPlatformWebApplication.DataAccess.Entities.CommentEntity", b =>
                {
                    b.HasOne("CreationsPlatformWebApplication.DataAccess.Entities.CreationEntity", "CreationEntity")
                        .WithMany("Comments")
                        .HasForeignKey("CreationId")
                        .IsRequired()
                        .HasConstraintName("creation_id_fk");

                    b.Navigation("CreationEntity");
                });

            modelBuilder.Entity("CreationsPlatformWebApplication.DataAccess.Entities.CreationEntity", b =>
                {
                    b.HasOne("CreationsPlatformWebApplication.DataAccess.Entities.UserEntity", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .IsRequired()
                        .HasConstraintName("author_id_fk");

                    b.Navigation("Author");
                });

            modelBuilder.Entity("CreationsPlatformWebApplication.DataAccess.Entities.CreationEntity", b =>
                {
                    b.Navigation("Comments");
                });
#pragma warning restore 612, 618
        }
    }
}
