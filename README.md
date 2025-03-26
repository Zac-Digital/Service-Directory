# Service Directory

This codebase is an example of a "Service Directory" written in .NET using the GOV.UK frontend. It includes a postcode
search and a large set of mocked services belonging to locations within the vicinities of them.

The intention of the project is to demonstrate a simple full stack .NET Application with a clean architecture approach
using an official frontend library, and is heavily inspired by a project I worked on named [Family Hubs](https://github.com/DFE-Digital/fh-services).

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

- `ServiceDirectory.Application` - Contains business logic on behalf of the Presentation layer(s), interfacing with the Infrastructure layer(s).
- `ServiceDirectory.Domain` - Contains POCOs (Plain Old CLR Objects) that is used to pass data between architectural layers.
- `ServiceDirectory.Infrastructure` - Contains external database connections and facilitates CRUD operations.
- `ServiceDirectory.Presentation.Api` - Contains an API that is consumed by the Web layer.
- `ServiceDirectory.Presentation.Web` - Contains the user-facing web application layer.

## Running

### Prerequisites

1. .NET 9 SDK

### Run

In a terminal*..

1. `cd` into the root of the project that contains `ServiceDirectory.sln`
2. Execute `dotnet restore`
3. Execute `dotnet build --configuration Release --no-restore`
4. `cd` into `src` and then `cd` into `ServiceDirectory.Presentation.Api`
5. Execute `dotnet run --configuration Release --no-build --no-restore`
6. In another terminal window/tab, `cd` into `src` and then `cd` into `ServiceDirectory.Presentation.Web`
7. Execute `dotnet run --configuration Release --no-build --no-restore`

You should now have the API and the Web Application running concurrently.

- Navigate to https://localhost:7024 to open the Service Directory Web Application and start playing around!

\* Your IDE likely has a couple of "Play" buttons to press, or you can make a multi-launch config, but these instructions
are designed to be universal and as such are platform and IDE agnostic.