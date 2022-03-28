namespace CompleteMinimalAPI.Models;
public record Provider
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Document { get; set; }
    public bool Active { get; set; }
}