namespace ServiceDirectory.Domain.Service;

public class Schedule : EntityBase
{
    public TimeOnly OpeningTime { get; init; }
    public TimeOnly ClosingTime { get; init; }
    public string DaysOfWeek { get; init; } = null!;
}