using Microsoft.AspNetCore.Mvc;
using ServiceDirectory.Domain.Postcode;
using ServiceDirectory.Presentation.Web.Client;
using ServiceDirectory.Presentation.Web.Pages.Shared;

namespace ServiceDirectory.Presentation.Web.Pages;

public class Results : ServiceDirectoryBasePage
{
    private readonly IApiClient _apiClient;

    public LocationModel Location { get; private set; } = null!;

    public Results(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<IActionResult> OnGetAsync(string postcode)
    {
        LocationModel? location = await _apiClient.GetLocationFromPostcode(postcode);
        
        if (location is null)
        {
            return Redirect("/Error");
        }
        
        Location = location;
        return Page();
    }
}