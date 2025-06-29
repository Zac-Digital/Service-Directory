using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceDirectory.Domain.Service;

public class Service : EntityBase
{
    public int OrganisationId { get; init; }
    
    public string Name { get; init; } = null!;
    public string Description { get; init; } = null!;
    public string Cost { get; init; } = null!;
    [NotMapped] public double DistanceInMiles { get; set; }
    
    public int ContactId { get; init; }
    public Contact Contact { get; init; } = null!;
    
    public int ScheduleId { get; init; }
    public Schedule Schedule { get; init; } = null!;
    public List<Location> Locations { get; init; } = [];
}
