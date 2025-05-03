namespace ServiceDirectory.Domain.Postcode;

public record PostcodeResultModel(Location Result);

public record Location(string Postcode, double Latitude, double Longitude);