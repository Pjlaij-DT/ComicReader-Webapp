namespace Panel.Api.Models;

public class ComicPage
{
    public long Id { get; set; }
    public long ChapterId { get; set; }
    public int PageNumber { get; set; }
    public string StorageKey { get; set; } = string.Empty;
    public string MediaType { get; set; } = string.Empty;
    public long ByteSize { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
    public string? AltText { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public ComicChapter Chapter { get; set; } = null!;
}
