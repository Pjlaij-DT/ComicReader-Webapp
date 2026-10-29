using Microsoft.AspNetCore.Mvc;
using Panel.Api.Models;

namespace Panel.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ComicsController : ControllerBase
{
    private static readonly List<Comic> Comics = 
    [
        new Comic
        {
            Id = 1,
            Title = "New Comic Test",
            Synopsis = "Test 1 comic",
            CoverImageUrl = "/images/sample-cover.jpg",
            Author = "DemoAuthor",
            Status = "Ongoing",
            Genres = {"Action", "Adventure"}
        },
        new Comic
        {
            Id = 2,
            Title = "The Last Kingdom",
            Synopsis = "A fantasy story about a fallen kingdom.",
            CoverImageUrl = "/images/kingdom-cover.jpg",
            Author = "Demo Author",
            Status = "Completed",
            Genres = {"Action", "Adventure"}
        }
    ];

    [HttpGet]
    public ActionResult<IEnumerable<Comic>> GetComics()
    {
        return Ok(Comics);
    }

    [HttpGet("{id:int}")]
    public ActionResult<Comic> GetComic(int id)
    {
        var comic = Comics.FirstOrDefault(c =>  c.Id == id);
        if (comic is null)
        {
            return NotFound();
        }
        
        return Ok(comic);
    }
}