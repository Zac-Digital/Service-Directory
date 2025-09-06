using NSubstitute;
using ServiceDirectory.Application.Postcode.Queries;
using ServiceDirectory.Domain.Postcode;
using ServiceDirectory.Infrastructure.Postcode;
using Shouldly;

namespace ServiceDirectory.Application.UnitTest.Postcode.Queries;

public sealed class PostcodeQueryTests
{
    private readonly PostcodeQuery _postcodeQuery;
    private readonly IPostcodeClient _postcodeClient;

    public PostcodeQueryTests()
    {
        _postcodeClient = Substitute.For<IPostcodeClient>();
        _postcodeQuery = new PostcodeQuery(_postcodeClient);
    }
    
    [Fact]
    public async Task GetLocationFromPostcode_ShouldReturnLocationInformation_When_PostcodeIsValid()
    {
        Location expectedResult = new Location("SW1A 1AA", 51.501009, -0.141588);
        _postcodeClient.GetLocation("SW1A 1AA")!.Returns(Task.FromResult(expectedResult));

        Location? result = await _postcodeQuery.GetLocationFromPostcode("SW1A 1AA");

        result.ShouldNotBeNull();
        result.ShouldBeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task GetLocationFromPostcode_ShouldReturnNull_When_PostcodeIsNotValid()
    {
        Location? expectedResult = null;
        _postcodeClient.GetLocation("INVALID").Returns(Task.FromResult(expectedResult));

        Location? result = await _postcodeQuery.GetLocationFromPostcode("INVALID");

        result.ShouldBeNull();
    }
}