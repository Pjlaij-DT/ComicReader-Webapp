using Microsoft.AspNetCore.Identity;

namespace Panel.Api.Models;

public class ApplicationUser : IdentityUser<Guid>
{
    public string DisplayName { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<Comment> ModeratedComments { get; set; } = new List<Comment>();
    public ICollection<Comic> CreatedComics { get; set; } = new List<Comic>();
    public ICollection<Comic> UpdatedComics { get; set; } = new List<Comic>();
    public ICollection<ReadingHistory> ReadingHistories { get; set; } = new List<ReadingHistory>();
}
