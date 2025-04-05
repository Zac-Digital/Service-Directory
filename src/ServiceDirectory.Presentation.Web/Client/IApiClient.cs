using ServiceDirectory.Domain.Postcode;

namespace ServiceDirectory.Presentation.Web.Client;

public interface IApiClient
{
    public Task<bool> IsPostcodeValid(string postcode);
    
    public Task<LocationModel?> GetLocationFromPostcode(string postcode);
}