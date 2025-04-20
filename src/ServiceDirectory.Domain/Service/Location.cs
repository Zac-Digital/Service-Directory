namespace ServiceDirectory.Domain.Service;

public class Location : EntityBase
{
    public int ServiceId { get; init; }
    
    public string AddressLineOne { get; init; } = null!;
    public string AddressLineTwo { get; init; } = null!;
    public string County { get; init; } = null!;
    public string TownOrCity { get; set; } = null!;
    public string Postcode { get; init; } = null!;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}