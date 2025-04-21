using Bogus;
using Microsoft.Extensions.Logging;
using ServiceDirectory.Domain.Service;
using ServiceDirectory.Infrastructure.Data;

namespace ServiceDirectory.Application.Database.Commands;

public class MockDataCommand : IMockDataCommand
{
    private readonly ILogger<IMockDataCommand> _logger;
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly Faker<Organisation> _mockOrganisation;

    public MockDataCommand(ILogger<IMockDataCommand> logger, IApplicationDbContext applicationDbContext)
    {
        const string locale = "en_GB";
        const int maxServicesPerOrganisation = 256;
        const int maxLocationsPerService = 8;

        _logger = logger;
        _applicationDbContext = applicationDbContext;

        Faker<Contact> mockContact = new Faker<Contact>(locale)
            .RuleFor(c => c.Email, f => f.Internet.Email())
            .RuleFor(c => c.Phone, f => f.Phone.PhoneNumber())
            .RuleFor(c => c.Website, f => f.Internet.Url());

        Faker<Schedule> mockSchedule = new Faker<Schedule>(locale)
            .RuleFor(s => s.OpeningTime, f => f.Date.BetweenTimeOnly(new TimeOnly(7, 0, 0, 0), new TimeOnly(9, 0, 0, 0)))
            .RuleFor(s => s.ClosingTime, f => f.Date.BetweenTimeOnly(new TimeOnly(15, 0, 0, 0), new TimeOnly(17, 0, 0, 0)))
            .RuleFor(s => s.DaysOfWeek, "MO,TU,WE,TH,FR"); // TODO: Randomise

        Faker<Location> mockLocation = new Faker<Location>(locale)
            .RuleFor(l => l.AddressLineOne, f => f.Address.StreetAddress())
            .RuleFor(l => l.AddressLineTwo, f => f.Address.SecondaryAddress())
            .RuleFor(l => l.County, f => f.Address.County())
            .RuleFor(l => l.Postcode, f => f.Address.ZipCode())
            .RuleFor(l => l.TownOrCity, f => f.Address.City())
            .RuleFor(l => l.Latitude, f => f.Address.Latitude(49.5, 60.5))
            .RuleFor(l => l.Longitude, f => f.Address.Longitude(-8.0, 2.0));

        Faker<Service> mockService = new Faker<Service>(locale)
            .RuleFor(s => s.Name, f => f.Company.CompanyName())
            .RuleFor(s => s.Description, f => f.Lorem.Paragraph())
            .RuleFor(s => s.Cost, f => f.Commerce.Price(0.00m, 29.99m))
            .RuleFor(s => s.Contact, _ => mockContact.Generate())
            .RuleFor(s => s.Schedule, _ => mockSchedule.Generate())
            .RuleFor(s => s.Locations, f => mockLocation.Generate(f.Random.Number(1, maxLocationsPerService)));

        _mockOrganisation = new Faker<Organisation>(locale)
            .RuleFor(o => o.Name, f => f.Company.CompanyName())
            .RuleFor(o => o.Description, f => f.Lorem.Paragraph())
            .RuleFor(o => o.Services, f => mockService.Generate(f.Random.Number(1, maxServicesPerOrganisation)));
    }

    public async Task<int> SeedDatabaseWithMockData()
    {
        const int maxNumberOfOrganisations = 32;

        _logger.LogInformation("Seeding Database with Mock Data...");

        _applicationDbContext.AddOrganisationRange(_mockOrganisation.Generate(maxNumberOfOrganisations));
        int numberOfRowsSaved = await _applicationDbContext.SaveChangesAsync();

        _logger.LogInformation("Created {NumberOfRowsChanged} Rows in the Database...", numberOfRowsSaved);
        _logger.LogInformation("Database Seeded Successfully!");

        return numberOfRowsSaved;
    }
}