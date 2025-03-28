using Microsoft.Playwright.Xunit;

namespace ServiceDirectory.Presentation.Web.E2E;

[Trait("Category", "E2E")]
public abstract class BasePageTest : PageTest
{
    protected async Task ElementByTestIdIsVisibleOnPage(string dataTestId)
        => await Expect(Page.GetByTestId(dataTestId)).ToBeVisibleAsync();
}