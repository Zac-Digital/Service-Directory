# Service Directory

This codebase is an example of a "Service Directory" written in .NET using the GOV.UK frontend. It includes a postcode
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
    |- ServiceDirectory.Infrastructure
    |- ServiceDirectory.Presentation.Api
    |- ServiceDirectory.Presentation.Web
```

- `ServiceDirectory.Application` - Contains business logic on behalf of the Presentation layer(s), interfacing with the
  Infrastructure layer(s).
- `ServiceDirectory.Domain` - Contains POCOs (Plain Old CLR Objects) that is used to pass data between architectural
  layers.
- `ServiceDirectory.Infrastructure` - Contains external database connections and facilitates CRUD operations.
- `ServiceDirectory.Presentation.Api` - Contains an API that is consumed by the Web layer.
- `ServiceDirectory.Presentation.Web` - Contains the user-facing web application layer.

## Building, Running & Testing

In a terminal*..

### Prerequisites

1. .NET 9 SDK with the ASP.NET Runtime

### Building

1. Make sure you have installed the [Prerequisites](#prerequisites)
2. `cd` into the root of the project that contains `ServiceDirectory.sln`
3. Execute `dotnet restore`
4. Execute `dotnet build --configuration Release --no-restore`

### Running

1. Make sure you have followed [Building](#building)
2. `cd` into `src` and then `cd` into `ServiceDirectory.Presentation.Api`
3. Execute `dotnet run --configuration Release --no-build --no-restore`
4. In another terminal window/tab, `cd` into `src` and then `cd` into `ServiceDirectory.Presentation.Web`
5. Execute `dotnet run --configuration Release --no-build --no-restore`

You should now have the API and the Web Application running concurrently.

- Navigate to https://localhost:7024 to open the Service Directory Web Application and start playing around!

### Testing

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