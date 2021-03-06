namespace CompleteMinimalAPI.Models;
public record Provider
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime LastUpdateDate { get; set; } = DateTime.UtcNow;
    public bool Active { get; set; }
}