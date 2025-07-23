namespace ServiceDirectory.Domain.Service;

public sealed class Schedule : EntityBase
{
    public TimeOnly OpeningTime { get; init; }
    public TimeOnly ClosingTime { get; init; }
    public string DaysOfWeek { get; init; } = null!;
}