using Microsoft.Playwright.Xunit;

namespace ServiceDirectory.Presentation.Web.E2E;

[Trait("Category", "E2E")]
public abstract class BasePageTest : PageTest
{
    private const string BaseUrl = "https://localhost:7024";

    protected string PageRelativeUrl { get; init; } = null!;
    protected string PageTitle { get; init; } = null!;
    protected string[] PageDataTestIdentifiers { get; set; } = null!;
    
    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        await Page.GotoAsync(BaseUrl + PageRelativeUrl);
    }

    [Fact]
    protected async Task HasTitle() => await Expect(Page).ToHaveTitleAsync(PageTitle);

    [Fact]
    public async Task HasContent()
    {
        foreach (string pageDataTestIdentifier in PageDataTestIdentifiers)
        {
            await Expect(Page.GetByTestId(pageDataTestIdentifier)).ToBeVisibleAsync();
        }
    }
}