using ServiceDirectory.Domain.Postcode;
using ServiceDirectory.Infrastructure.Postcode;

namespace ServiceDirectory.Application.Postcode.Queries;

public sealed class PostcodeQuery : IPostcodeQuery
{
    private readonly IPostcodeClient _postcodeClient;

    public PostcodeQuery(IPostcodeClient postcodeClient)
    {
        _postcodeClient = postcodeClient;
    }

    public Task<Location?> GetLocationFromPostcode(string postcode) => _postcodeClient.GetLocation(postcode);
}