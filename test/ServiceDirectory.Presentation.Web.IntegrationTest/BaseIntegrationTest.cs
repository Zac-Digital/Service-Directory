using Microsoft.Extensions.DependencyInjection;
using ServiceDirectory.Infrastructure.Data;
using ServiceDirectory.Presentation.Web.IntegrationTest.Service.Queries;

namespace ServiceDirectory.Presentation.Web.IntegrationTest;

public class BaseIntegrationTest : CustomWebApplicationFactory
{
    private IServiceScope? _scope;

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        _scope?.Dispose();
    }

    private ApplicationDbContext GetDbContext()
    {
        _scope = Services.CreateScope();
        return _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    }

    protected async Task<ApplicationDbContext> GetDbContextWithOneOrganisationWithOneService()
    {
        ApplicationDbContext dbContext = GetDbContext();
        dbContext.AddOrganisationRange([new TestData().TestOrganisationWithSingleService]);
        await dbContext.SaveChangesAsync();
        return dbContext;
    }

    protected ApplicationDbContext GetDbContextWithNoOrganisationWithNoService() => GetDbContext();
}