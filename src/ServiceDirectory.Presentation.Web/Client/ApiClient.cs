using System.Text.Json;
using ServiceDirectory.Domain.Postcode;

namespace ServiceDirectory.Presentation.Web.Client;

public class ApiClient : IApiClient
{
    private readonly ILogger<IApiClient> _logger;
    private readonly HttpClient _httpClient;

    public ApiClient(ILogger<IApiClient> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    public async Task<bool> IsPostcodeValid(string postcode)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"/postcode/validate/{postcode}");

        if (response.IsSuccessStatusCode)
        {
            return JsonSerializer.Deserialize<bool>(await response.Content.ReadAsStringAsync(),
                JsonSerializerOptions.Web);
        }

        _logger.LogError("Error: Request with URL {URL} failed with status code: {StatusCode}",
            response.RequestMessage?.RequestUri, response.StatusCode);

        return false;
    }

    public async Task<LocationModel?> GetLocationFromPostcode(string postcode)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"/postcode/location/{postcode}");

        if (response.IsSuccessStatusCode)
        {
            return JsonSerializer.Deserialize<LocationModel>(await response.Content.ReadAsStringAsync(),
                JsonSerializerOptions.Web);
        }

        _logger.LogError("Error: Request with URL {URL} failed with status code: {StatusCode}",
            response.RequestMessage?.RequestUri, response.StatusCode);

        return null;
    }
}