namespace ServiceDirectory.Domain.Service;

public class Contact : EntityBase
{
    public string Email { get; init; } = null!;
    public string Phone { get; init; } = null!;
    public string Website { get; init; } = null!;
}