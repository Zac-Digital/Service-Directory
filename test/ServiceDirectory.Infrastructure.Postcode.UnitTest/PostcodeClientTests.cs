using System.Net;
using Microsoft.Extensions.Logging;
using NSubstitute;
using ServiceDirectory.Domain.Postcode;
using ServiceDirectory.Infrastructure.Postcode.UnitTest.Helpers;
using Shouldly;

namespace ServiceDirectory.Infrastructure.Postcode.UnitTest;

public class PostcodeClientTests
{
    [Fact]
    public async Task GetLocation_Returns_ExpectedResult()
    {
        Location expectedResult = new Location("SW1A 1AA", 51.501009, -0.141588);

        MockHttpMessageHandler mockHttpMessageHandler = new MockHttpMessageHandler(HttpStatusCode.OK,
            "{\"status\":200,\"result\":{\"postcode\":\"SW1A 1AA\",\"quality\":1,\"eastings\":529090,\"northings\":179645,\"country\":\"England\",\"nhs_ha\":\"London\",\"longitude\":-0.141588,\"latitude\":51.501009,\"european_electoral_region\":\"London\",\"primary_care_trust\":\"Westminster\",\"region\":\"London\",\"lsoa\":\"Westminster 018C\",\"msoa\":\"Westminster 018\",\"incode\":\"1AA\",\"outcode\":\"SW1A\",\"parliamentary_constituency\":\"Cities of London and Westminster\",\"parliamentary_constituency_2024\":\"Cities of London and Westminster\",\"admin_district\":\"Westminster\",\"parish\":\"Westminster, unparished area\",\"admin_county\":null,\"date_of_introduction\":\"198001\",\"admin_ward\":\"St James's\",\"ced\":null,\"ccg\":\"NHS North West London\",\"nuts\":\"Westminster\",\"pfa\":\"Metropolitan Police\",\"codes\":{\"admin_district\":\"E09000033\",\"admin_county\":\"E99999999\",\"admin_ward\":\"E05013806\",\"parish\":\"E43000236\",\"parliamentary_constituency\":\"E14001172\",\"parliamentary_constituency_2024\":\"E14001172\",\"ccg\":\"E38000256\",\"ccg_id\":\"W2U3Z\",\"ced\":\"E99999999\",\"nuts\":\"TLI32\",\"lsoa\":\"E01004736\",\"msoa\":\"E02000977\",\"lau2\":\"E09000033\",\"pfa\":\"E23000001\"}}}");
        HttpClient httpClient = new HttpClient(mockHttpMessageHandler) { BaseAddress = new Uri("http://localhost/") };
        PostcodeClient postcodeClient = new PostcodeClient(Substitute.For<ILogger<IPostcodeClient>>(), httpClient);
        
        Location? result = await postcodeClient.GetLocation("SW1A 1AA");

        result.ShouldNotBeNull();
        result.ShouldBeEquivalentTo(expectedResult);
    }
    
    [Fact]
    public async Task GetLocation_ReturnsNull_If_OutgoingStatusCode_IsNot_OK()
    {
        MockHttpMessageHandler mockHttpMessageHandler =
            new MockHttpMessageHandler(HttpStatusCode.BadGateway, "");
        HttpClient httpClient = new HttpClient(mockHttpMessageHandler) { BaseAddress = new Uri("http://localhost/") };
        PostcodeClient postcodeClient = new PostcodeClient(Substitute.For<ILogger<IPostcodeClient>>(), httpClient);

        Location? result = await postcodeClient.GetLocation("SW1A 1AA");

        result.ShouldBeNull();
    }
    
    [Fact]
    public async Task GetLocation_ReturnsNull_If_DeserialisedModel_Is_Null()
    {
        MockHttpMessageHandler mockHttpMessageHandler =
            new MockHttpMessageHandler(HttpStatusCode.OK, "null");
        HttpClient httpClient = new HttpClient(mockHttpMessageHandler) { BaseAddress = new Uri("http://localhost/") };
        PostcodeClient postcodeClient = new PostcodeClient(Substitute.For<ILogger<IPostcodeClient>>(), httpClient);

        Location? result = await postcodeClient.GetLocation("SW1A 1AA");

        result.ShouldBeNull();
    }
}