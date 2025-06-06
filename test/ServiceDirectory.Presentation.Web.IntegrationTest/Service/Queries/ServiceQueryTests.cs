using ServiceDirectory.Application.Services.Queries;
using ServiceDirectory.Domain.ServiceList;
using Shouldly;

namespace ServiceDirectory.Presentation.Web.IntegrationTest.Service.Queries;

public class ServiceQueryTests : BaseIntegrationTest
{
    [Fact]
    public async Task GetServicesByLocation_Should_ReturnServices()
    {
        ServiceQuery serviceQuery = new ServiceQuery(await GetDbContextWithOneOrganisationWithOneService());
        
        ServiceList result = serviceQuery.GetServicesByLocation(51.503399, -0.127874, 1);
        
        result.Total.ShouldBe(1);
        result.Services.Count.ShouldBe(1);
        result.Services[0].Name.ShouldBe("Test Service #1");
        result.Services[0].Description.ShouldBe("Test Description #1");
        result.Services[0].Cost.ShouldBe("9.99");
        result.Services[0].DistanceInMiles.ShouldBeGreaterThan(0.0d);
        result.Services[0].Schedule.OpeningTime.ShouldBe(TimeOnly.MinValue);
        result.Services[0].Schedule.ClosingTime.ShouldBe(TimeOnly.MaxValue);
        result.Services[0].Schedule.DaysOfWeek.ShouldBe("MO,TU,WE,TH,FR,SA,SU");
    }

    [Fact]
    public async Task GetServicesByLocation_Should_BeEmpty_If_NoServiceIsWithinRange()
    {
        ServiceQuery serviceQuery = new ServiceQuery(await GetDbContextWithOneOrganisationWithOneService());
        
        ServiceList result = serviceQuery.GetServicesByLocation(0.0d, -0.0d, 1);

        result.Total.ShouldBe(0);
        result.Services.Count.ShouldBe(0);
    }

    [Fact]
    public void GetServicesByLocation_Should_BeEmpty_If_ThereAreNoServices()
    {
        ServiceQuery serviceQuery = new ServiceQuery(GetDbContextWithNoOrganisationWithNoService());
        
        ServiceList result = serviceQuery.GetServicesByLocation(51.503399, -0.127874, 1);
        
        result.Total.ShouldBe(0);
        result.Services.Count.ShouldBe(0);
    }
}