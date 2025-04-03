namespace ServiceDirectory.Presentation.Web.Client;

public interface IApiClient
{
    public Task<bool> IsPostcodeValid(string postcode);
}