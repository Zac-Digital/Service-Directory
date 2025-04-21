namespace ServiceDirectory.Domain.Service;

public class Organisation : EntityBase
{
    public string Name { get; init; } = null!;
    public string Description { get; init; } = null!;

    public List<Service> Services { get; init; } = [];
}