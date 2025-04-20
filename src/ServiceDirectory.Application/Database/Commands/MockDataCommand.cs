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

    private static readonly List<(string Name, double Latitude, double Longitude)> UkPopulatedAreas =
    [
        ("London", 51.5074, -0.1278),
        ("Birmingham", 52.4862, -1.8904),
        ("Liverpool", 53.4084, -2.9916),
        ("Glasgow", 55.8642, -4.2518),
        ("Manchester", 53.4808, -2.2426),
        ("Sheffield", 53.3811, -1.4701),
        ("Leeds", 53.7997, -1.5492),
        ("Edinburgh", 55.9533, -3.1883),
        ("Bristol", 51.4545, -2.5879),
        ("Leicester", 52.6369, -1.1398),
        ("Coventry", 52.4068, -1.5197),
        ("Bradford", 53.7960, -1.7594),
        ("Cardiff", 51.4816, -3.1791),
        ("Belfast", 54.5973, -5.9301),
        ("Nottingham", 52.9548, -1.1581),
        ("Kingston upon Hull", 53.7446, -0.3367),
        ("Newcastle upon Tyne", 54.9783, -1.6178),
        ("Stoke-on-Trent", 53.0027, -2.1794),
        ("Southampton", 50.9097, -1.4044),
        ("Derby", 52.9225, -1.4746),
        ("Portsmouth", 50.8198, -1.0880),
        ("Brighton", 50.8225, -0.1372),
        ("Plymouth", 50.3755, -4.1427),
        ("Wolverhampton", 52.5870, -2.1284),
        ("Aberdeen", 57.1497, -2.0943),
        ("Reading", 51.4543, -0.9781),
        ("Swansea", 51.6214, -3.9436),
        ("Milton Keynes", 52.0406, -0.7594),
        ("Northampton", 52.2304, -0.8969),
        ("Luton", 51.8787, -0.4200),
        ("Sunderland", 54.9061, -1.3811),
        ("Warrington", 53.3900, -2.5976),
        ("Huddersfield", 53.6458, -1.7850),
        ("Bournemouth", 50.7192, -1.8808),
        ("Peterborough", 52.5695, -0.2405),
        ("York", 53.9599, -1.0873),
        ("Dundee", 56.4620, -2.9707),
        ("Birkenhead", 53.3933, -3.0143),
        ("Middlesbrough", 54.5742, -1.2350),
        ("Oxford", 51.7520, -1.2577),
        ("Bolton", 53.5785, -2.4299),
        ("Ipswich", 52.0567, 1.1482),
        ("Cambridge", 52.2053, 0.1218),
        ("Telford", 52.6784, -2.4453),
        ("Preston", 53.7632, -2.7031),
        ("Blackpool", 53.8175, -3.0357),
        ("Norwich", 52.6309, 1.2974),
        ("Stourbridge", 52.4567, -2.1484),
        ("Exeter", 50.7236, -3.5275),
        ("Solihull", 52.4118, -1.7776),
        ("Crawley", 51.1092, -0.1872),
        ("Cheltenham", 51.8979, -2.0583),
        ("Blackburn", 53.7465, -2.4849),
        ("Basildon", 51.5762, 0.4702),
        ("Slough", 51.5105, -0.5950),
        ("Colchester", 51.8959, 0.8919),
        ("Gillingham", 51.3856, 0.5443),
        ("Newport", 51.5842, -2.9977),
        ("Woking", 51.3190, -0.5576),
        ("Maidstone", 51.2705, 0.5228),
        ("Basingstoke", 51.2667, -1.0876),
        ("Swindon", 51.5561, -1.7794),
        ("Grimsby", 53.5871, -0.0686)
    ];

    public MockDataCommand(ILogger<IMockDataCommand> logger, IApplicationDbContext applicationDbContext)
    {
        const int maxServicesPerOrganisation = 32;
        const int maxLocationsPerService = 4;

        _logger = logger;
        _applicationDbContext = applicationDbContext;

        Faker<Contact> mockContact = new Faker<Contact>("en_GB")
            .RuleFor(c => c.Email, f => f.Internet.Email())
            .RuleFor(c => c.Phone, f => f.Phone.PhoneNumber())
            .RuleFor(c => c.Website, f => f.Internet.Url());

        Faker<Schedule> mockSchedule = new Faker<Schedule>("en_GB")
            .RuleFor(s => s.OpeningTime, f => f.Date.BetweenTimeOnly(new TimeOnly(7, 0), new TimeOnly(9, 0)))
            .RuleFor(s => s.ClosingTime, f => f.Date.BetweenTimeOnly(new TimeOnly(15, 0), new TimeOnly(17, 0)))
            .RuleFor(s => s.DaysOfWeek, "MO,TU,WE,TH,FR");

        Faker<Location> mockLocation = new Faker<Location>("en_GB")
            .RuleFor(l => l.AddressLineOne, f => f.Address.StreetAddress())
            .RuleFor(l => l.AddressLineTwo, f => f.Address.SecondaryAddress())
            .RuleFor(l => l.County, f => f.Address.County())
            .RuleFor(l => l.Postcode, f => f.Address.ZipCode())
            .Rules((f, l) =>
            {
                (string name, double latitude, double longitude) = f.PickRandom(UkPopulatedAreas);

                double latitudeOffset = f.Random.Double(-0.025, 0.025);
                double longitudeOffset = f.Random.Double(-0.025, 0.025);

                l.TownOrCity = name;
                l.Latitude = latitude + latitudeOffset;
                l.Longitude = longitude + longitudeOffset;
            });

        Faker<Service> mockService = new Faker<Service>("en_GB")
            .RuleFor(s => s.Name, f => f.Company.CompanyName())
            .RuleFor(s => s.Description, f => f.Lorem.Paragraph())
            .RuleFor(s => s.Cost, f => double.Parse(f.Commerce.Price((decimal)0.00, (decimal)29.99d)))
            .RuleFor(s => s.Contact, f => mockContact.Generate())
            .RuleFor(s => s.Schedule, f => mockSchedule.Generate())
            .RuleFor(s => s.Locations, f => mockLocation.Generate(f.Random.Number(1, maxLocationsPerService)));

        _mockOrganisation = new Faker<Organisation>("en_GB")
            .RuleFor(o => o.Name, f => f.Company.CompanyName())
            .RuleFor(o => o.Description, f => f.Lorem.Paragraph())
            .RuleFor(o => o.Services, f => mockService.Generate(f.Random.Number(1, maxServicesPerOrganisation)));
    }

    public async Task SeedDatabaseWithMockData()
    {
        const int maxNumberOfOrganisations = 8;

        _logger.LogInformation("Seeding Database with Mock Data...");

        List<Organisation> mockOrganisationList = _mockOrganisation.Generate(maxNumberOfOrganisations);

        // foreach (Location mockLocation in mockOrganisationList.SelectMany(mockOrganisation => 
        //              mockOrganisation.Services.SelectMany(mockService => mockService.Locations)))
        // {
        //     LocationModel postcodeLocation = await GetRandomPostcodeLocation();
        //
        //     mockLocation.Postcode = postcodeLocation.Postcode;
        //     mockLocation.Latitude = postcodeLocation.Latitude;
        //     mockLocation.Longitude = postcodeLocation.Longitude;
        // }

        _logger.LogInformation("Database Seeded Successfully!");
    }
}