using System.Collections.Specialized;
using System.Web;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using ServiceDirectory.Application.Services.Queries;
using ServiceDirectory.Domain.Service;
using ServiceDirectory.Domain.ServiceList;
using ServiceDirectory.Presentation.Web.Pages.Shared;
using Location = ServiceDirectory.Domain.Postcode.Location;

namespace ServiceDirectory.Presentation.Web.Pages;

public class Results : ServiceDirectoryBasePage
{
    private readonly IServiceQuery _serviceQuery;

    public string? Postcode { get; private set; }

    public IEnumerable<Service> Services { get; private set; } = [];
    public bool LocationHasAtLeastOneService { get; private set; }

    [BindProperty(SupportsGet = true)] public int CurrentPage { get; set; }
    public int TotalPages { get; set; }

    public Results(IServiceQuery serviceQuery)
    {
        CurrentPage = 1;
        _serviceQuery = serviceQuery;
    }

    public IActionResult OnGet(Location location, int currentPage)
    {
        CurrentPage = currentPage;
        Postcode = location.Postcode;

        ServiceList searchResult =
            _serviceQuery.GetServicesByLocation(location.Latitude, location.Longitude, CurrentPage);

        Services = searchResult.Services;
        LocationHasAtLeastOneService = Services.Any();

        TotalPages = (searchResult.Total + 10 - 1) / 10;

        return Page();
    }

    public string? GetNavigationHref(int pageOffset)
    {
        RouteValueDictionary routeValues = new RouteValueDictionary();
    
        foreach (KeyValuePair<string, StringValues> queryParameter in Request.Query)
        {
            routeValues[queryParameter.Key] = queryParameter.Value.ToString();
        }
    
        routeValues[nameof(CurrentPage)] = (CurrentPage + pageOffset).ToString();
    
        return Url.Page("/Results", routeValues);

    }
}