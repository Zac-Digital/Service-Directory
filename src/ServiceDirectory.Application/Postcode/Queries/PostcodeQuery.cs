using ServiceDirectory.Domain.Postcode;
using ServiceDirectory.Infrastructure.Postcode;

namespace ServiceDirectory.Application.Postcode.Queries;

public class PostcodeQuery : IPostcodeQuery
{
    private readonly IPostcodeClient _postcodeClient;

    public PostcodeQuery(IPostcodeClient postcodeClient)
    {
        _postcodeClient = postcodeClient;
    }

    public Task<bool> IsPostcodeValid(string postcode) => _postcodeClient.ValidatePostcode(postcode);
    
    public Task<LocationModel?> GetLocationFromPostcode(string postcode) => _postcodeClient.GetLocation(postcode);
}