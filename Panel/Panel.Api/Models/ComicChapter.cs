namespace Panel.Api.Models;

public class ComicChapter
{
    public long Id { get; set; }
    public int ComicId { get; set; }
    public string ChapterLabel { get; set; } = string.Empty;
    public string? Title { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsPublished { get; set; }
    public DateTime? PublishedAtUtc { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public Comic Comic { get; set; } = null!;
    public ICollection<ComicPage> Pages { get; set; } = new List<ComicPage>();
}
