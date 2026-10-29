using Microsoft.Net.Http.Headers;

namespace Panel.Api.Models;

public class Comic
{
    public int Id {get; set; }
    public string Title {get; set; } = string.Empty;
    public string Synopsis {get; set;} = string.Empty;
    public string CoverImageUrl { get; set; } = string.Empty;

    public string Author { get; set; } = string.Empty;

    public string Status { get; set; } = "Ongoing";
    public List<string> Genres{get; set;} = new List<string>();
    
}