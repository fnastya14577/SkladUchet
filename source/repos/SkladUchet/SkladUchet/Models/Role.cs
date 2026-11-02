namespace SkladUchet.Models;

public class Role
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string? Description { get; set; }
    public int AccessLevel { get; set; } = 1;
    public List<User> Users { get; set; } = new();
}