using ServiceDirectory.Application.Postcode.Queries;

namespace ServiceDirectory.Presentation.Api.Endpoints;

public class MinimalPostcodeEndpoints
{
    public void Register(WebApplication webApplication)
    {
        webApplication.MapGet("/postcode/validate/{postcode}",
                async (string postcode, IPostcodeQuery postcodeQuery) => await postcodeQuery.IsPostcodeValid(postcode))
            .WithOpenApi()
            .WithDescription("Check if a postcode is valid.");

        webApplication.MapGet("/postcode/location/{postcode}",
            async (string postcode, IPostcodeQuery postcodeQuery) =>
                await postcodeQuery.GetLocationFromPostcode(postcode))
            .WithOpenApi()
            .WithDescription("Get the user's latitude and longitude from a postcode.");
    }
}