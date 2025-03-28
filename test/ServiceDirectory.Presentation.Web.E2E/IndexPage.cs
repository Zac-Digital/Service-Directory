namespace ServiceDirectory.Presentation.Web.E2E;

public class IndexPage : BasePageTest
{
    private static readonly string[] PageElements =
    [
        "Index_HEADER_Service-Directory",

        "Index_PARAGRAPH_Main-Example",
        "Index_PARAGRAPH_Secondary-Example",

        "Index_LIST_Example",
        "Index_LIST-ITEM_Example-One",
        "Index_LIST-ITEM_Example-Two",
        "Index_LIST-ITEM_Example-Three",
        "Index_LIST-ITEM_Example-Four",

        "Index_BUTTON_Start-Now"
    ];

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        await Page.GotoAsync("https://localhost:7024");
    }

    [Fact]
    public async Task HasTitle()
    {
        await Expect(Page).ToHaveTitleAsync("Start Page - GOV.UK");
    }

    [Fact]
    public async Task HasContent()
    {
        foreach (string pageElement in PageElements)
        {
            await ElementByTestIdIsVisibleOnPage(pageElement);
        }
    }
}