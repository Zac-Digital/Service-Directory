using Microsoft.AspNetCore.Mvc;
using ServiceDirectory.Domain.Postcode;
using ServiceDirectory.Presentation.Web.Pages.Shared;

namespace ServiceDirectory.Presentation.Web.Pages;

public class Results : ServiceDirectoryBasePage
{
    public Location Location { get; private set; } = null!;
    
    public IActionResult OnGet(Location location)
    {
        Location = location;
        return Page();
    }
}