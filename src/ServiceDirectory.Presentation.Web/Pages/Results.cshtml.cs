using Microsoft.AspNetCore.Mvc;
using ServiceDirectory.Application.Services.Queries;
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

    public Results(IServiceQuery serviceQuery)
    {
        _serviceQuery = serviceQuery;
    }
    
    public IActionResult OnGet(Location location)
    {
        Postcode = location.Postcode;
        Services = _serviceQuery.GetServicesByLocation(location.Latitude, location.Longitude).ToList();
        LocationHasAtLeastOneService = Services.Count > 0;
        
        return Page();
    }
}
