using ServiceDirectory.Domain.Postcode;

namespace ServiceDirectory.Application.Postcode.Queries;

public interface IPostcodeQuery
{
    public Task<bool> IsPostcodeValid(string postcode);
    
    public Task<LocationModel?> GetLocationFromPostcode(string postcode);
}