using NSubstitute;
using ServiceDirectory.Application.Postcode.Queries;
using ServiceDirectory.Domain.Postcode;
using ServiceDirectory.Infrastructure.Postcode;
using Shouldly;

namespace ServiceDirectory.Application.Unit.Postcode.Queries;

public class PostcodeQueryTests
{
    private readonly PostcodeQuery _postcodeQuery;
    private readonly IPostcodeClient _postcodeClient;

    public PostcodeQueryTests()
    {
        _postcodeClient = Substitute.For<IPostcodeClient>();
        _postcodeQuery = new PostcodeQuery(_postcodeClient);
    }

    [Fact]
    public async Task IsPostcodeValid_ShouldReturnTrue_When_PostcodeIsValid()
    {
        _postcodeClient.ValidatePostcode("SW1A 1AA").Returns(Task.FromResult(true));

        bool result = await _postcodeQuery.IsPostcodeValid("SW1A 1AA");
        
        result.ShouldBeTrue();
    }
    
    [Fact]
    public async Task IsPostcodeValid_ShouldReturnFalse_When_PostcodeIsNotValid()
    {
        _postcodeClient.ValidatePostcode("INVALID").Returns(Task.FromResult(false));

        bool result = await _postcodeQuery.IsPostcodeValid("INVALID");
        
        result.ShouldBeFalse();
    }

    [Fact]
    public async Task GetLocationFromPostcode_ShouldReturnLocationInformation_When_PostcodeIsValid()
    {
        LocationModel expectedResult = new LocationModel("SW1A 1AA", 51.501009, -0.141588);
        _postcodeClient.GetLocation("SW1A 1AA")!.Returns(Task.FromResult(expectedResult));
        
        LocationModel? result = await _postcodeQuery.GetLocationFromPostcode("SW1A 1AA");

        result.ShouldNotBeNull();
        result.ShouldBeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task GetLocationFromPostcode_ShouldReturnNull_When_PostcodeIsNotValid()
    {
        LocationModel? expectedResult = null;
        _postcodeClient.GetLocation("INVALID").Returns(Task.FromResult(expectedResult));
        
        LocationModel? result = await _postcodeQuery.GetLocationFromPostcode("INVALID");
        
        result.ShouldBeNull();
    }
}