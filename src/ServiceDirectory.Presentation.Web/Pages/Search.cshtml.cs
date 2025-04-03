using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
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
        if (!ModelState.IsValid)
        {
            Error = true;
            return Page();
        }
        
        bool isValidPostcode = await _apiClient.IsPostcodeValid(Postcode!);

        if (!isValidPostcode)
        {
            Error = true;
            return Page();
        }
        
        throw new NotImplementedException(); // TODO: Implement Next Page
    }
}