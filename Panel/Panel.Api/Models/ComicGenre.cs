namespace Panel.Api.Models;

public class ComicGenre
{
    public int ComicId { get; set; }
    public int GenreId { get; set; }

    public Comic Comic { get; set; } = null!;
    public Genre Genre { get; set; } = null!;
}
