using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceDirectory.Application.Postcode.Queries;
using ServiceDirectory.Domain.Postcode;
using ServiceDirectory.Presentation.Web.Pages.Shared;

namespace ServiceDirectory.Presentation.Web.Pages;

public sealed class Search : ServiceDirectoryBasePage
{
    private readonly IPostcodeQuery _postcodeQuery;

    public Search(IPostcodeQuery postcodeQuery)
    {
        _postcodeQuery = postcodeQuery;
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
        
        Location? location = await _postcodeQuery.GetLocationFromPostcode(Postcode);

        if (location is null)
        {
            return PageWithErrorState();
        }
        
        return RedirectToPage("/Results", new { location.Postcode, location.Latitude, location.Longitude, CurrentPage = 1 });
    }

    private PageResult PageWithErrorState()
    {
        Error = true;
        return Page();
    }
}
