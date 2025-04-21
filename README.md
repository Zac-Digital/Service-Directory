# Service Directory

This codebase is an example of a "Service Directory" written in .NET using the [GOV.UK frontend](https://github.com/alphagov/govuk-frontend), following the [GDS](https://design-system.service.gov.uk/) as faithfully as possible. It includes a postcode
search and a large set of mocked services belonging to locations within the vicinities of them.

The intention of the project is to demonstrate a simple full stack .NET Application with a clean architecture approach
using an official frontend library, and is heavily inspired by a project I worked on
named [Family Hubs](https://github.com/DFE-Digital/fh-services).

## Structure

```
.
|- src
    |- ServiceDirectory.Application
    |- ServiceDirectory.Domain
    |- ServiceDirectory.Infrastructure.Data
    |- ServiceDirectory.Infrastructure.Postcode
    |- ServiceDirectory.Presentation.Api
    |- ServiceDirectory.Presentation.Web
```

- `ServiceDirectory.Application` - Contains business logic on behalf of the Presentation layer(s), interfacing with the
  Infrastructure layer(s).
- `ServiceDirectory.Domain` - Contains POCOs (Plain Old CLR Objects) that is used to pass data between architectural
  layers.
- `ServiceDirectory.Infrastructure.Data` - Contains external database connections and facilitates CRUD operations.
- `ServiceDirectory.Infrastructure.Postcode` - Contains an HTTP client repository connecting to [Postcodes.io](https://postcodes.io/)
- `ServiceDirectory.Presentation.Api` - Contains an API that is consumed by the Web layer.
- `ServiceDirectory.Presentation.Web` - Contains the user-facing web application layer.

## Building, Running & Testing

In a terminal*..

### Prerequisites

1. .NET 9 SDK with the ASP.NET Runtime
2. Docker

### Database

This project uses Microsoft SQL Server 2022 for the Database. There is a convenient Docker Compose file that will get
you up and running quickly:

1. `cd` into `utility` and then `cd` into `mssql`
2. Execute `docker compose up -d`

This will work for Windows, macOS and Linux. If you are on Windows (x86-64) specifically, you may also choose to
install Microsoft SQL Server 2022 natively — but I recommend going with Docker as it's easier to manage.

### Building

1. Make sure you have installed the [Prerequisites](#prerequisites)
2. Make sure you have got the [Database](#database) running
3. `cd` into the root of the project that contains `ServiceDirectory.sln`
4. Execute `dotnet restore`
5. Execute `dotnet build --configuration Release --no-restore`

### Running

1. Make sure you have followed [Building](#building)
2. `cd` into `src` and then `cd` into `ServiceDirectory.Presentation.Api`
3. Execute `dotnet run --configuration Release --no-build --no-restore`
4. In another terminal window/tab, `cd` into `src` and then `cd` into `ServiceDirectory.Presentation.Web`
5. Execute `dotnet run --configuration Release --no-build --no-restore`

You should now have the API and the Web Application running concurrently.

- Navigate to https://localhost:7024/ to open the Service Directory Web Application and start playing around the UI!
- Navigate to https://localhost:7086/swagger/index.html to open the Service Directory API and play with the endpoints!

### Testing

Please note that Docker is **required** for running the Integration Tests, as they use [Testcontainers](https://testcontainers.com/?language=dotnet).

1. Make sure you have followed [Building](#building)
2. `cd` into the root of the project that contains `ServiceDirectory.sln`
3. To run **WITH** the E2E Tests..
    1. Make sure you have the app up and running: [Running](#running)
    2. Ensure [PowerShell](https://learn.microsoft.com/en-us/powershell/scripting/install/installing-powershell?view=powershell-7.5) is installed, and execute `pwsh bin/Release/net9.0/playwright.ps1 install`
    3. Execute `dotnet test --configuration Release --no-build --no-restore`
4. To run **WITHOUT** the E2E Tests..
    1. Execute `dotnet test --configuration Release --no-build --no-restore --filter Category!=E2E`

\* Your IDE likely has a couple of "Play" or "Test" buttons to press, or you can make a multi-launch config, but these
instructions are designed to be universal and as such are platform and IDE agnostic.
