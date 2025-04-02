using System.Net;
using Microsoft.Extensions.Logging;
using NSubstitute;
using ServiceDirectory.Infrastructure.Postcode.Unit.Helpers;
using Shouldly;

namespace ServiceDirectory.Infrastructure.Postcode.Unit;

public class PostcodeClientTests
{
    [Theory]
    [InlineData("true")]
    [InlineData("false")]
    public async Task ValidatePostcode_Returns_ExpectedResult(string expectedResult)
    {
        MockHttpMessageHandler mockHttpMessageHandler =
            new MockHttpMessageHandler(HttpStatusCode.OK, $"{{\"status\":200,\"result\":{expectedResult}}}");
        HttpClient httpClient = new HttpClient(mockHttpMessageHandler) { BaseAddress = new Uri("http://localhost/") };
        PostcodeClient postcodeClient = new PostcodeClient(Substitute.For<ILogger<IPostcodeClient>>(), httpClient);

        bool result = await postcodeClient.ValidatePostcode("SW1A 1AA");

        result.ShouldBe(bool.Parse(expectedResult));
    }

    [Fact]
    public async Task ValidatePostcode_ReturnsFalse_If_OutgoingStatusCode_IsNot_OK()
    {
        MockHttpMessageHandler mockHttpMessageHandler =
            new MockHttpMessageHandler(HttpStatusCode.BadGateway, "");
        HttpClient httpClient = new HttpClient(mockHttpMessageHandler) { BaseAddress = new Uri("http://localhost/") };
        PostcodeClient postcodeClient = new PostcodeClient(Substitute.For<ILogger<IPostcodeClient>>(), httpClient);

        bool result = await postcodeClient.ValidatePostcode("SW1A 1AA");

        result.ShouldBeFalse();
    }

    [Fact]
    public async Task ValidatePostcode_ReturnsFalse_If_DeserialisedModel_Is_Null()
    {
        MockHttpMessageHandler mockHttpMessageHandler =
            new MockHttpMessageHandler(HttpStatusCode.OK, "null");
        HttpClient httpClient = new HttpClient(mockHttpMessageHandler) { BaseAddress = new Uri("http://localhost/") };
        PostcodeClient postcodeClient = new PostcodeClient(Substitute.For<ILogger<IPostcodeClient>>(), httpClient);

        bool result = await postcodeClient.ValidatePostcode("SW1A 1AA");

        result.ShouldBeFalse();
    }

    [Fact]
    public async Task ValidatePostCode_ReturnsFalse_If_ExternalService_Had_AnError()
    {
        MockHttpMessageHandler mockHttpMessageHandler =
            new MockHttpMessageHandler(HttpStatusCode.OK, "{\"status\":400,\"result\":false}");
        HttpClient httpClient = new HttpClient(mockHttpMessageHandler) { BaseAddress = new Uri("http://localhost/") };
        PostcodeClient postcodeClient = new PostcodeClient(Substitute.For<ILogger<IPostcodeClient>>(), httpClient);

        bool result = await postcodeClient.ValidatePostcode("SW1A 1AA");

        result.ShouldBeFalse();
    }
}