namespace Panel.Api.Dtos;

public class UserDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Avatar { get; set; } = string.Empty;
    public string Status { get; set; } = "Type1";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
