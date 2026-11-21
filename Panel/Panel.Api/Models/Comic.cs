namespace Panel.Api.Models;

public class Comic
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Synopsis { get; set; } = string.Empty;
    public string? CoverImageUrl { get; set; }
    public string Author { get; set; } = string.Empty;
    public string Status { get; set; } = "Ongoing";
    public bool IsArchived { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAtUtc { get; set; }
    public Guid? CreatedByUserId { get; set; }
    public Guid? UpdatedByUserId { get; set; }

    public ApplicationUser? CreatedByUser { get; set; }
    public ApplicationUser? UpdatedByUser { get; set; }
    public ICollection<ComicGenre> ComicGenres { get; set; } = new List<ComicGenre>();
    public ICollection<ComicChapter> Chapters { get; set; } = new List<ComicChapter>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<ReadingHistory> ReadingHistories { get; set; } = new List<ReadingHistory>();
}