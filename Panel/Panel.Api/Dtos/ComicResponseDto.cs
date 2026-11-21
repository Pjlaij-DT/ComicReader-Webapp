namespace Panel.Api.Dtos;

public class ComicResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Synopsis { get; set; } = string.Empty;
    public string? CoverImageUrl { get; set; }
    public string Author { get; set; } = string.Empty;
    public string Status { get; set; } = "Ongoing";
    public List<string> Genres { get; set; } = new();
}
