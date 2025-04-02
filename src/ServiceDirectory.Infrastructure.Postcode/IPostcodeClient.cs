namespace ServiceDirectory.Infrastructure.Postcode;

public interface IPostcodeClient
{
    public Task<bool> ValidatePostcode(string postcode);
}