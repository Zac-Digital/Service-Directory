using NSubstitute;
using ServiceDirectory.Application.Postcode.Queries;
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
}