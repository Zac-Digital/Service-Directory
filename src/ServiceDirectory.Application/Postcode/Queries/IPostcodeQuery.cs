namespace ServiceDirectory.Application.Postcode.Queries;

public interface IPostcodeQuery
{
    public Task<bool> IsPostcodeValid(string postcode);
}