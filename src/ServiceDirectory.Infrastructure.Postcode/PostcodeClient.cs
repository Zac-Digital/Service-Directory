using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using ServiceDirectory.Domain.Postcode;

namespace ServiceDirectory.Infrastructure.Postcode;

public class PostcodeClient : IPostcodeClient
{
    private const int StatusCodeOk = (int)HttpStatusCode.OK;

    private readonly ILogger<IPostcodeClient> _logger;
    private readonly HttpClient _httpClient;

    public PostcodeClient(ILogger<IPostcodeClient> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    public async Task<bool> ValidatePostcode(string postcode)
    {
        // Stryker disable once all : External service is mocked in testing so URL does not matter
        HttpResponseMessage response = await _httpClient.GetAsync($"/postcodes/{postcode}/validate");

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Error: Request with URL {URL} failed with status code: {StatusCode}",
                response.RequestMessage?.RequestUri, response.StatusCode);
            return false;
        }

        ValidateModel? model = JsonSerializer.Deserialize<ValidateModel>(await response.Content.ReadAsStringAsync(),
            JsonSerializerOptions.Web);

        if (model is null)
        {
            _logger.LogError("Error: Attempting to deserialise result returned: NULL");
            return false;
        }

        if (model.Status != StatusCodeOk)
        {
            _logger.LogWarning("Warning: Response was successful, but external provider had an error: {PostcodeStatus}",
                model.Status);

            return false;
        }

        _logger.LogInformation("Success: Postcode validation returned: {Result}", model.Result);

        return model.Result;
    }

    public async Task<LocationModel?> GetLocation(string postcode)
    {
        // Stryker disable once all : External service is mocked in testing so URL does not matter
        HttpResponseMessage response = await _httpClient.GetAsync($"/postcodes/{postcode}");

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Error: Request with URL {URL} failed with status code: {StatusCode}",
                response.RequestMessage?.RequestUri, response.StatusCode);
            return null;
        }

        PostcodeResultModel? postcodeResultModel =
            JsonSerializer.Deserialize<PostcodeResultModel>(await response.Content.ReadAsStringAsync(),
                JsonSerializerOptions.Web);

        if (postcodeResultModel is null)
        {
            _logger.LogError("Error: Attempting to deserialise result returned: NULL");
            return null;
        }
        
        _logger.LogInformation("Success: Postcode lookup returned: {Result}", postcodeResultModel.Result);

        return postcodeResultModel.Result;
    }
}