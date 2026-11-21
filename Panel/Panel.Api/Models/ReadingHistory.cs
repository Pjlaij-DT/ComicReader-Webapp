namespace Panel.Api.Models;

public class ReadingHistory
{
    public Guid UserId { get; set; }
    public int ComicId { get; set; }
    public DateTime FirstReadAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime LastReadAtUtc { get; set; } = DateTime.UtcNow;

    public ApplicationUser User { get; set; } = null!;
    public Comic Comic { get; set; } = null!;
}
