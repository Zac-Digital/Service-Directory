namespace ServiceDirectory.Domain.Postcode;

public sealed record PostcodeResultModel(Location Result);

public sealed record Location(string Postcode, double Latitude, double Longitude);