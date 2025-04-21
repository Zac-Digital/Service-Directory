using ServiceDirectory.Domain.Postcode;

namespace ServiceDirectory.Infrastructure.Postcode;

public interface IPostcodeClient
{
    public Task<bool> ValidatePostcode(string postcode);
    
    public Task<LocationModel?> GetLocation(string postcode);
}