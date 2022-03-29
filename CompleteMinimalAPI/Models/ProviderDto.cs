namespace CompleteMinimalAPI.Models;

public class ProviderDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public bool Active { get; set; }
}