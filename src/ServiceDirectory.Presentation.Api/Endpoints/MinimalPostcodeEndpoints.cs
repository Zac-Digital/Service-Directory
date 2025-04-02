using ServiceDirectory.Application.Postcode.Queries;

namespace ServiceDirectory.Presentation.Api.Endpoints;

public class MinimalPostcodeEndpoints
{
    public void Register(WebApplication webApplication)
    {
        webApplication.MapGet("/postcode/validate/{postcode}",
                async (string postcode, IPostcodeQuery postcodeQuery) => await postcodeQuery.IsPostcodeValid(postcode))
            .WithOpenApi();
    }
}