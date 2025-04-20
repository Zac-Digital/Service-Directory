namespace ServiceDirectory.Application.Database.Commands;

public interface IMockDataCommand
{
    public Task SeedDatabaseWithMockData();
}