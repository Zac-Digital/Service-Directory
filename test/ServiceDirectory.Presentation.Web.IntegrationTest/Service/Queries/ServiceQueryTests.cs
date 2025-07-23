using ServiceDirectory.Application.Services.Queries;
using ServiceDirectory.Domain.ServiceList;
using Shouldly;

namespace ServiceDirectory.Presentation.Web.IntegrationTest.Service.Queries;

public sealed class ServiceQueryTests : BaseIntegrationTest
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
    public async Task GetServicesByLocation_Should_Return_ASinglePage_Of_Services()
    {
        ServiceQuery serviceQuery = new ServiceQuery(await GetDbContextWithOneOrganisationWithMultipleServices());
        
        ServiceList result = serviceQuery.GetServicesByLocation(51.503399, -0.127874, 1);
        
        result.Total.ShouldBe(32);
        result.Services.Count.ShouldBe(10);
        result.Services[0].Name.ShouldBe("Test Service #1");
        result.Services[0].Description.ShouldBe("Test Description #1");
        result.Services[0].Cost.ShouldBe("9.99");
        result.Services[0].DistanceInMiles.ShouldBeGreaterThan(0.0d);
        result.Services[0].Schedule.OpeningTime.ShouldBe(TimeOnly.MinValue);
        result.Services[0].Schedule.ClosingTime.ShouldBe(TimeOnly.MaxValue);
        result.Services[0].Schedule.DaysOfWeek.ShouldBe("MO,TU,WE,TH,FR,SA,SU");
    }
    
    [Fact]
    public async Task GetServicesByLocation_Should_Paginate_Services()
    {
        ServiceQuery serviceQuery = new ServiceQuery(await GetDbContextWithOneOrganisationWithMultipleServices());

        ServiceList result = serviceQuery.GetServicesByLocation(51.503399, -0.127874, 1);
        
        result.Total.ShouldBe(32);
        result.Services.Count.ShouldBe(10);
        result.Services[0].Name.ShouldBe("Test Service #1");
        result.Services[0].Description.ShouldBe("Test Description #1");
        
        result = serviceQuery.GetServicesByLocation(51.503399, -0.127874, 2);
        
        result.Total.ShouldBe(32);
        result.Services.Count.ShouldBe(10);
        result.Services[0].Name.ShouldBe("Test Service #11");
        result.Services[0].Description.ShouldBe("Test Description #11");
        
        result = serviceQuery.GetServicesByLocation(51.503399, -0.127874, 3);
        
        result.Total.ShouldBe(32);
        result.Services.Count.ShouldBe(10);
        result.Services[0].Name.ShouldBe("Test Service #21");
        result.Services[0].Description.ShouldBe("Test Description #21");
        
        result = serviceQuery.GetServicesByLocation(51.503399, -0.127874, 4);
        
        result.Total.ShouldBe(32);
        result.Services.Count.ShouldBe(2);
        result.Services[0].Name.ShouldBe("Test Service #31");
        result.Services[0].Description.ShouldBe("Test Description #31");
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