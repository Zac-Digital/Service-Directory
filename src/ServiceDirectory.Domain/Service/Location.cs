namespace ServiceDirectory.Domain.Service;

public sealed class Location : EntityBase
{
    public string AddressLineOne { get; init; } = null!;
    public string AddressLineTwo { get; init; } = null!;
    public string County { get; init; } = null!;
    public string TownOrCity { get; init; } = null!;
    public string Postcode { get; init; } = null!;
    public double Latitude { get; init; }
    public double Longitude { get; init; }
}