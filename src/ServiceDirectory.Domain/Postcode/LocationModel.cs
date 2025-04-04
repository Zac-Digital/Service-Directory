namespace ServiceDirectory.Domain.Postcode;

public record PostcodeResultModel(LocationModel Result);

public record LocationModel(string Postcode, double Latitude, double Longitude);