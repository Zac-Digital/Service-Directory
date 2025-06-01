using Microsoft.AspNetCore.Mvc;
using ServiceDirectory.Application.Services.Queries;
using ServiceDirectory.Domain.Result;
using ServiceDirectory.Domain.Service;
using ServiceDirectory.Presentation.Web.Pages.Shared;
using Location = ServiceDirectory.Domain.Postcode.Location;

namespace ServiceDirectory.Presentation.Web.Pages;

public class Results : ServiceDirectoryBasePage
{
    private readonly IServiceQuery _serviceQuery;
    
    public string? Postcode { get; private set; }

    public List<Service> Services { get; private set; } = [];
    public bool LocationHasAtLeastOneService { get; private set; }
    
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }

    public Results(IServiceQuery serviceQuery)
    {
        CurrentPage = 1;
        _serviceQuery = serviceQuery;
    }
    
    public IActionResult OnGet(Location location)
    {
        Postcode = location.Postcode;
        
        Result searchResult = _serviceQuery.GetServicesByLocation(location.Latitude, location.Longitude);
        
        Services = searchResult.Services.ToList();
        LocationHasAtLeastOneService = Services.Count > 0;

        TotalPages = searchResult.Total;
        
        return Page();
    }

    public IActionResult OnPost()
    {
        return Page();
    }
}
