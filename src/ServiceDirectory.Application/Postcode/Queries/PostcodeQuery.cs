using ServiceDirectory.Infrastructure.Postcode;

namespace ServiceDirectory.Application.Postcode.Queries;

public class PostcodeQuery : IPostcodeQuery
{
    private readonly IPostcodeClient _postcodeClient;

    public PostcodeQuery(IPostcodeClient postcodeClient)
    {
        _postcodeClient = postcodeClient;
    }

    public async Task<bool> IsPostcodeValid(string postcode) => await _postcodeClient.ValidatePostcode(postcode);
}