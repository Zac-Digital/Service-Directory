using ServiceDirectory.Domain.Postcode;

namespace ServiceDirectory.Presentation.Web.Client;

public interface IApiClient
{
    public Task<Location?> GetLocationFromPostcode(string postcode);
}