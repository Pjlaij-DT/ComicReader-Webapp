namespace Panel.Api.Models;

public class Comment
{
    public long Id { get; set; }
    public int ComicId { get; set; }
    public Guid? AuthorUserId { get; set; }
    public long? ParentCommentId { get; set; }
    public string? Content { get; set; }
    public CommentModerationStatus ModerationStatus { get; set; } = CommentModerationStatus.Visible;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? EditedAtUtc { get; set; }
    public Guid? ModeratedByUserId { get; set; }
    public DateTime? ModeratedAtUtc { get; set; }
    public string? ModerationReason { get; set; }

    public Comic Comic { get; set; } = null!;
    public ApplicationUser? AuthorUser { get; set; }
    public ApplicationUser? ModeratedByUser { get; set; }
    public Comment? ParentComment { get; set; }
    public ICollection<Comment> Replies { get; set; } = new List<Comment>();
}
