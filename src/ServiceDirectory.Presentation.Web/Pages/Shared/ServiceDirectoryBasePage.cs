using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceDirectory.Presentation.Web.Pages.Shared;

public abstract class ServiceDirectoryBasePage : PageModel
{
    public bool Error { get; set; } = false;
}