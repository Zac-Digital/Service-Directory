using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceDirectory.Domain.Postcode;
using ServiceDirectory.Presentation.Web.Client;
using ServiceDirectory.Presentation.Web.Pages.Shared;

namespace ServiceDirectory.Presentation.Web.Pages;

public class Search : ServiceDirectoryBasePage
{
    private readonly IApiClient _apiClient;

    public Search(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }
    
    [Required]
    [StringLength(8)]
    [BindProperty]
    public string? Postcode { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        if (string.IsNullOrWhiteSpace(Postcode))
        {
            return PageWithErrorState();
        }
        
        Location? location = await _apiClient.GetLocationFromPostcode(Postcode);

        if (location is null)
        {
            return PageWithErrorState();
        }
        
        return RedirectToPage("/Results", new { location.Postcode, location.Latitude, location.Longitude });
    }

    private PageResult PageWithErrorState()
    {
        Error = true;
        return Page();
    }
}