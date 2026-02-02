namespace PracticeLogger.Domain.Models;

public record Session(int Id, DateOnly Date, string Activity, int Minutes, string? Notes)
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}
