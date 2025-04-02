using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceDirectory.Presentation.Web.Pages;

public class Search : PageModel
{
    [Required]
    [StringLength(8)]
    [BindProperty]
    public string? Postcode { get; set; }
    
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        
        throw new NotImplementedException(); // TODO: Implement Next Page
    }
}