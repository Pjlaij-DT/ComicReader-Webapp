using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Panel.Api.Models;

namespace Panel.Api.Data;

public class PanelDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    public PanelDbContext(DbContextOptions<PanelDbContext> options)
        : base(options)
    {
    }

    public DbSet<Comic> Comics => Set<Comic>();
    public DbSet<Genre> Genres => Set<Genre>();
    public DbSet<ComicGenre> ComicGenres => Set<ComicGenre>();
    public DbSet<ComicChapter> ComicChapters => Set<ComicChapter>();
    public DbSet<ComicPage> ComicPages => Set<ComicPage>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<ReadingHistory> ReadingHistories => Set<ReadingHistory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.Property(user => user.DisplayName)
                .HasMaxLength(80)
                .IsRequired();
            entity.Property(user => user.AvatarUrl)
                .HasColumnType("text");
            entity.Property(user => user.CreatedAtUtc)
                .HasColumnName("CreatedAtUtc")
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.HasIndex(user => user.NormalizedEmail)
                .IsUnique()
                .HasDatabaseName("EmailIndex");
        });

        modelBuilder.Entity<Comic>(entity =>
        {
            entity.ToTable("comics", table =>
                table.HasCheckConstraint(
                    "ck_comics_status",
                    "status IN ('Ongoing', 'Completed')"));

            entity.HasKey(comic => comic.Id);
            entity.Property(comic => comic.Id)
                .HasColumnName("id")
                .UseIdentityByDefaultColumn();
            entity.Property(comic => comic.Title)
                .HasColumnName("title")
                .HasMaxLength(200)
                .IsRequired();
            entity.Property(comic => comic.Synopsis)
                .HasColumnName("synopsis")
                .HasColumnType("text")
                .IsRequired();
            entity.Property(comic => comic.CoverImageUrl)
                .HasColumnName("cover_image_url")
                .HasColumnType("text");
            entity.Property(comic => comic.Author)
                .HasColumnName("author")
                .HasMaxLength(200)
                .IsRequired();
            entity.Property(comic => comic.Status)
                .HasColumnName("status")
                .HasMaxLength(32)
                .HasDefaultValue("Ongoing")
                .IsRequired();
            entity.Property(comic => comic.IsArchived)
                .HasColumnName("is_archived")
                .HasDefaultValue(false);
            entity.Property(comic => comic.CreatedAtUtc)
                .HasColumnName("created_at_utc")
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(comic => comic.UpdatedAtUtc)
                .HasColumnName("updated_at_utc")
                .HasColumnType("timestamp with time zone");
            entity.Property(comic => comic.CreatedByUserId)
                .HasColumnName("created_by_user_id");
            entity.Property(comic => comic.UpdatedByUserId)
                .HasColumnName("updated_by_user_id");
            entity.HasIndex(comic => new { comic.IsArchived, comic.Status, comic.Id })
                .HasDatabaseName("ix_comics_public_list");
            entity.HasOne(comic => comic.CreatedByUser)
                .WithMany(user => user.CreatedComics)
                .HasForeignKey(comic => comic.CreatedByUserId)
                .OnDelete(DeleteBehavior.SetNull);
            entity.HasOne(comic => comic.UpdatedByUser)
                .WithMany(user => user.UpdatedComics)
                .HasForeignKey(comic => comic.UpdatedByUserId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.ToTable("genres");
            entity.HasKey(genre => genre.Id);
            entity.Property(genre => genre.Id)
                .HasColumnName("id")
                .UseIdentityByDefaultColumn();
            entity.Property(genre => genre.Name)
                .HasColumnName("name")
                .HasMaxLength(80)
                .IsRequired();
            entity.Property(genre => genre.Slug)
                .HasColumnName("slug")
                .HasMaxLength(100)
                .IsRequired();
            entity.HasIndex(genre => genre.Slug)
                .IsUnique()
                .HasDatabaseName("ix_genres_slug");
        });

        modelBuilder.Entity<ComicGenre>(entity =>
        {
            entity.ToTable("comic_genres");
            entity.HasKey(comicGenre => new { comicGenre.ComicId, comicGenre.GenreId });
            entity.Property(comicGenre => comicGenre.ComicId).HasColumnName("comic_id");
            entity.Property(comicGenre => comicGenre.GenreId).HasColumnName("genre_id");
            entity.HasIndex(comicGenre => new { comicGenre.GenreId, comicGenre.ComicId })
                .HasDatabaseName("ix_comic_genres_genre_id_comic_id");
            entity.HasOne(comicGenre => comicGenre.Comic)
                .WithMany(comic => comic.ComicGenres)
                .HasForeignKey(comicGenre => comicGenre.ComicId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(comicGenre => comicGenre.Genre)
                .WithMany(genre => genre.ComicGenres)
                .HasForeignKey(comicGenre => comicGenre.GenreId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ComicChapter>(entity =>
        {
            entity.ToTable("comic_chapters", table =>
                table.HasCheckConstraint("ck_comic_chapters_display_order", "display_order > 0"));
            entity.HasKey(chapter => chapter.Id);
            entity.Property(chapter => chapter.Id)
                .HasColumnName("id")
                .UseIdentityByDefaultColumn();
            entity.Property(chapter => chapter.ComicId).HasColumnName("comic_id");
            entity.Property(chapter => chapter.ChapterLabel)
                .HasColumnName("chapter_label")
                .HasMaxLength(32)
                .IsRequired();
            entity.Property(chapter => chapter.Title)
                .HasColumnName("title")
                .HasMaxLength(200);
            entity.Property(chapter => chapter.DisplayOrder).HasColumnName("display_order");
            entity.Property(chapter => chapter.IsPublished)
                .HasColumnName("is_published")
                .HasDefaultValue(false);
            entity.Property(chapter => chapter.PublishedAtUtc)
                .HasColumnName("published_at_utc")
                .HasColumnType("timestamp with time zone");
            entity.Property(chapter => chapter.CreatedAtUtc)
                .HasColumnName("created_at_utc")
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.HasIndex(chapter => new { chapter.ComicId, chapter.DisplayOrder })
                .IsUnique()
                .HasDatabaseName("ux_comic_chapters_comic_order");
            entity.HasIndex(chapter => new { chapter.ComicId, chapter.IsPublished, chapter.DisplayOrder })
                .HasDatabaseName("ix_comic_chapters_published_order");
            entity.HasOne(chapter => chapter.Comic)
                .WithMany(comic => comic.Chapters)
                .HasForeignKey(chapter => chapter.ComicId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ComicPage>(entity =>
        {
            entity.ToTable("comic_pages", table =>
            {
                table.HasCheckConstraint("ck_comic_pages_page_number", "page_number > 0");
                table.HasCheckConstraint("ck_comic_pages_byte_size", "byte_size > 0");
                table.HasCheckConstraint("ck_comic_pages_width", "width IS NULL OR width > 0");
                table.HasCheckConstraint("ck_comic_pages_height", "height IS NULL OR height > 0");
            });
            entity.HasKey(page => page.Id);
            entity.Property(page => page.Id)
                .HasColumnName("id")
                .UseIdentityByDefaultColumn();
            entity.Property(page => page.ChapterId).HasColumnName("chapter_id");
            entity.Property(page => page.PageNumber).HasColumnName("page_number");
            entity.Property(page => page.StorageKey)
                .HasColumnName("storage_key")
                .HasMaxLength(512)
                .IsRequired();
            entity.Property(page => page.MediaType)
                .HasColumnName("media_type")
                .HasMaxLength(100)
                .IsRequired();
            entity.Property(page => page.ByteSize).HasColumnName("byte_size");
            entity.Property(page => page.Width).HasColumnName("width");
            entity.Property(page => page.Height).HasColumnName("height");
            entity.Property(page => page.AltText)
                .HasColumnName("alt_text")
                .HasMaxLength(500);
            entity.Property(page => page.CreatedAtUtc)
                .HasColumnName("created_at_utc")
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.HasIndex(page => new { page.ChapterId, page.PageNumber })
                .IsUnique()
                .HasDatabaseName("ux_comic_pages_chapter_page_number");
            entity.HasIndex(page => page.StorageKey)
                .IsUnique()
                .HasDatabaseName("ux_comic_pages_storage_key");
            entity.HasOne(page => page.Chapter)
                .WithMany(chapter => chapter.Pages)
                .HasForeignKey(page => page.ChapterId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.ToTable("comments", table =>
            {
                table.HasCheckConstraint(
                    "ck_comments_moderation_status",
                    "moderation_status IN ('Visible', 'Hidden', 'Removed')");
                table.HasCheckConstraint(
                    "ck_comments_visible_content",
                    "moderation_status <> 'Visible' OR (content IS NOT NULL AND length(btrim(content)) > 0)");
            });
            entity.HasKey(comment => comment.Id);
            entity.Property(comment => comment.Id)
                .HasColumnName("id")
                .UseIdentityByDefaultColumn();
            entity.Property(comment => comment.ComicId).HasColumnName("comic_id");
            entity.Property(comment => comment.AuthorUserId).HasColumnName("author_user_id");
            entity.Property(comment => comment.ParentCommentId).HasColumnName("parent_comment_id");
            entity.Property(comment => comment.Content)
                .HasColumnName("content")
                .HasColumnType("text");
            entity.Property(comment => comment.ModerationStatus)
                .HasColumnName("moderation_status")
                .HasConversion<string>()
                .HasMaxLength(16);
            entity.Property(comment => comment.CreatedAtUtc)
                .HasColumnName("created_at_utc")
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(comment => comment.EditedAtUtc)
                .HasColumnName("edited_at_utc")
                .HasColumnType("timestamp with time zone");
            entity.Property(comment => comment.ModeratedByUserId).HasColumnName("moderated_by_user_id");
            entity.Property(comment => comment.ModeratedAtUtc)
                .HasColumnName("moderated_at_utc")
                .HasColumnType("timestamp with time zone");
            entity.Property(comment => comment.ModerationReason)
                .HasColumnName("moderation_reason")
                .HasColumnType("text");
            entity.HasAlternateKey(comment => new { comment.ComicId, comment.Id })
                .HasName("ak_comments_comic_id_id");
            entity.HasIndex(comment => new { comment.ComicId, comment.CreatedAtUtc, comment.Id })
                .HasDatabaseName("ix_comments_comic_created_id");
            entity.HasIndex(comment => comment.ParentCommentId)
                .HasDatabaseName("ix_comments_parent_comment_id");
            entity.HasOne(comment => comment.Comic)
                .WithMany(comic => comic.Comments)
                .HasForeignKey(comment => comment.ComicId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(comment => comment.AuthorUser)
                .WithMany(user => user.Comments)
                .HasForeignKey(comment => comment.AuthorUserId)
                .OnDelete(DeleteBehavior.SetNull);
            entity.HasOne(comment => comment.ModeratedByUser)
                .WithMany(user => user.ModeratedComments)
                .HasForeignKey(comment => comment.ModeratedByUserId)
                .OnDelete(DeleteBehavior.SetNull);
            entity.HasOne(comment => comment.ParentComment)
                .WithMany(parent => parent.Replies)
                .HasForeignKey(comment => new { comment.ComicId, comment.ParentCommentId })
                .HasPrincipalKey(parent => new { parent.ComicId, parent.Id })
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ReadingHistory>(entity =>
        {
            entity.ToTable("reading_histories");
            entity.HasKey(history => new { history.UserId, history.ComicId });
            entity.Property(history => history.UserId).HasColumnName("user_id");
            entity.Property(history => history.ComicId).HasColumnName("comic_id");
            entity.Property(history => history.FirstReadAtUtc)
                .HasColumnName("first_read_at_utc")
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(history => history.LastReadAtUtc)
                .HasColumnName("last_read_at_utc")
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.HasIndex(history => new { history.UserId, history.LastReadAtUtc })
                .HasDatabaseName("ix_reading_histories_user_last_read");
            entity.HasOne(history => history.User)
                .WithMany(user => user.ReadingHistories)
                .HasForeignKey(history => history.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(history => history.Comic)
                .WithMany(comic => comic.ReadingHistories)
                .HasForeignKey(history => history.ComicId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
