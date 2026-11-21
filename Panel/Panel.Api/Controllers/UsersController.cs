
using Microsoft.AspNetCore.Mvc;
using Panel.Api.Dtos;

namespace Panel.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    // Temporary in-memory storage
    private static readonly List<UserDto> Users = new()
    {
        new UserDto
        {
            Id = 1,
            Name = "admin",
            Email = "admin@panel.com",
            Avatar = "/image/check.png",
            Status = "Panel administrator"
        },
        new UserDto
        {
            Id = 2,
            Name = "reader01",
            Email = "reader@example.com",
            Avatar = "/image/check2.pngr",
            Status = "I love reading manga!"
        }
    };

    private static int _nextId = 3;

    // GET: api/users
    [HttpGet]
    public ActionResult<IEnumerable<UserDto>> GetUsers()
    {
        return Ok(Users);
    }

    // GET: api/users/1
    [HttpGet("{id:int}")]
    public ActionResult<UserDto> GetUser(int id)
    {
        var user = Users.FirstOrDefault(u => u.Id == id);

        if (user == null)
        {
            return NotFound(new
            {
                message = $"User with ID {id} was not found."
            });
        }

        return Ok(user);
    }

    // POST: api/users
    [HttpPost]
    public ActionResult<UserDto> CreateUser(UserDto user)
    {
        // Check whether username already exists
        if (Users.Any(u =>
            u.Name.Equals(
                user.Name,
                StringComparison.OrdinalIgnoreCase)))
        {
            return Conflict(new
            {
                message = "Username already exists."
            });
        }

        // Check whether email already exists
        if (Users.Any(u =>
            u.Email.Equals(
                user.Email,
                StringComparison.OrdinalIgnoreCase)))
        {
            return Conflict(new
            {
                message = "Email already exists."
            });
        }

        user.Id = _nextId++;
        user.CreatedAt = DateTime.UtcNow;

        Users.Add(user);

        return CreatedAtAction(
            nameof(GetUser),
            new { id = user.Id },
            user
        );
    }

    // PUT: api/users/1
    [HttpPut("{id:int}")]
    public ActionResult<UserDto> UpdateUser(int id, UserDto updatedUser)
    {
        var user = Users.FirstOrDefault(u => u.Id == id);

        if (user == null)
        {
            return NotFound(new
            {
                message = $"User with ID {id} was not found."
            });
        }

        // Check if another user owns this username
        if (Users.Any(u =>
            u.Id != id &&
            u.Name.Equals(
                updatedUser.Name,
                StringComparison.OrdinalIgnoreCase)))
        {
            return Conflict(new
            {
                message = "Username already exists."
            });
        }

        // Check if another user owns this email
        if (Users.Any(u =>
            u.Id != id &&
            u.Email.Equals(
                updatedUser.Email,
                StringComparison.OrdinalIgnoreCase)))
        {
            return Conflict(new
            {
                message = "Email already exists."
            });
        }

        user.Name = updatedUser.Name;
        user.Email = updatedUser.Email;
        user.Status = updatedUser.Status;

        return Ok(user);
    }

    // DELETE: api/users/1
    [HttpDelete("{id:int}")]
    public IActionResult DeleteUser(int id)
    {
        var user = Users.FirstOrDefault(u => u.Id == id);

        if (user == null)
        {
            return NotFound(new
            {
                message = $"User with ID {id} was not found."
            });
        }

        Users.Remove(user);

        return NoContent();
    }
}
