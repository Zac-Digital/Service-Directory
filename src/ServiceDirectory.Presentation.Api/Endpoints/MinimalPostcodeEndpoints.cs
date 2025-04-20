using ServiceDirectory.Application.Database.Commands;
using ServiceDirectory.Application.Postcode.Queries;

namespace ServiceDirectory.Presentation.Api.Endpoints;

public class MinimalPostcodeEndpoints
{
    public void Register(WebApplication webApplication)
    {
        webApplication.MapGet("/postcode/validate/{postcode}",
                async (string postcode, IPostcodeQuery postcodeQuery) => await postcodeQuery.IsPostcodeValid(postcode))
            .WithOpenApi();

        webApplication.MapGet("/postcode/location/{postcode}",
            async (string postcode, IPostcodeQuery postcodeQuery) =>
                await postcodeQuery.GetLocationFromPostcode(postcode)).WithOpenApi();

        webApplication.MapPost("/database/mock",
            async (IMockDataCommand mockDataCommand) => await mockDataCommand.SeedDatabaseWithMockData()).WithOpenApi();
    }
}