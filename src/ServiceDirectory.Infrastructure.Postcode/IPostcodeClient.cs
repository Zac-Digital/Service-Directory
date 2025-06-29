using ServiceDirectory.Domain.Postcode;

namespace ServiceDirectory.Infrastructure.Postcode;

public interface IPostcodeClient
{
    public Task<Location?> GetLocation(string postcode);
}