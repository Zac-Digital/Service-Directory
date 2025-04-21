using System.Net;
using Shouldly;

namespace ServiceDirectory.Application.IntegrationTest.Database.Commands;

public class MockDataCommandTests : CustomWebApplicationFactory
{
    [Fact]
    public async Task ServiceSeedData_Should_SeedData()
    {
        HttpResponseMessage result = await HttpClient.PostAsync("/service/seed-data", null);
        
        result.StatusCode.ShouldBe(HttpStatusCode.OK);
        
        bool isInteger = int.TryParse(await result.Content.ReadAsStringAsync(), out int numberOfRowsSaved);
        
        isInteger.ShouldBeTrue();
        numberOfRowsSaved.ShouldBeGreaterThan(0);
    }
}