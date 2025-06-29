using ServiceDirectory.Domain.Postcode;

namespace ServiceDirectory.Application.Postcode.Queries;

public interface IPostcodeQuery
{
    public Task<Location?> GetLocationFromPostcode(string postcode);
}