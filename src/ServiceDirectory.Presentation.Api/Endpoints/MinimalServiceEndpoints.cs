using ServiceDirectory.Application.Database.Commands;

namespace ServiceDirectory.Presentation.Api.Endpoints;

public class MinimalServiceEndpoints
{
    public void Register(WebApplication webApplication)
    {
        webApplication.MapPost("/service/seed-data",
            async (IMockDataCommand mockDataCommand) => await mockDataCommand.SeedDatabaseWithMockData())
            .WithOpenApi()
            .WithDescription("Seed the database with mock data.");
    }
}