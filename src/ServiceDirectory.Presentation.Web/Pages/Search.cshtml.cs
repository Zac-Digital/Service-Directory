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
        Task<bool> isPostcodeValid = _apiClient.IsPostcodeValid(Postcode!);
        
        if (ModelState.IsValid && await isPostcodeValid)
        {
            return RedirectToPage("/Results", new { Postcode });
        }
        
        Error = true;
        return Page();

    }
}