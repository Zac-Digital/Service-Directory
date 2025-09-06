using System.Net;

namespace ServiceDirectory.Infrastructure.Postcode.UnitTest.Helpers;

public sealed class MockHttpMessageHandler : HttpMessageHandler
{
    private readonly HttpStatusCode _statusCode;
    private readonly string _response;

    public MockHttpMessageHandler(HttpStatusCode statusCode, string response)
    {
        _statusCode = statusCode;
        _response = response;
    }

    protected override Task<HttpResponseMessage>
        SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) =>
        Task.FromResult(new HttpResponseMessage { StatusCode = _statusCode, Content = new StringContent(_response) });
}