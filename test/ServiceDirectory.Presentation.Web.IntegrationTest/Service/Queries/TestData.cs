using ServiceDirectory.Domain.Service;

namespace ServiceDirectory.Presentation.Web.IntegrationTest.Service.Queries;

public class TestData
{
    public readonly Organisation TestOrganisationWithSingleService = new()
    {
        Name = "Test Organisation #1",
        Description = "Test Organisation Description #1",
        Services =
        [
            new Domain.Service.Service
            {
                Name = "Test Service #1",
                Description = "Test Description #1",
                Cost = "9.99",
                Contact = new Contact
                {
                    Email = "test@email.co.uk",
                    Phone = "01234567890",
                    Website = "https://www.example.com/"
                },
                Schedule = new Schedule
                {
                    OpeningTime = TimeOnly.MinValue,
                    ClosingTime = TimeOnly.MaxValue,
                    DaysOfWeek = "MO,TU,WE,TH,FR,SA,SU"
                },
                Locations =
                [
                    new Location
                    {
                        AddressLineOne = "128 Test Street",
                        AddressLineTwo = "Test Apartment 256",
                        County = "Testshire",
                        TownOrCity = "Test Town",
                        Postcode = "SW1A 1AA",
                        Latitude = 51.501009,
                        Longitude = -0.141588
                    }
                ]
            }
        ]
    };
}