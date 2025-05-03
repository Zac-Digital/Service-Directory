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

    public async Task<Location?> GetLocationFromPostcode(string postcode)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"/postcode/location/{postcode}");

        if (response.IsSuccessStatusCode)
        {
            return JsonSerializer.Deserialize<Location>(await response.Content.ReadAsStringAsync(),
                JsonSerializerOptions.Web);
        }

        _logger.LogError("Error: Request with URL {URL} failed with status code: {StatusCode}",
            response.RequestMessage?.RequestUri, response.StatusCode);

        return null;
    }
}