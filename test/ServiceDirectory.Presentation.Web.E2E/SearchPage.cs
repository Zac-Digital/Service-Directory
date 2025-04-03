namespace ServiceDirectory.Presentation.Web.E2E;

public class SearchPage : BasePageTest
{
    public SearchPage()
    {
        PageRelativeUrl = "/Search";
        PageTitle = "Search Page - GOV.UK";
        PageDataTestIdentifiers =
        [
            "Search_LABEL_Primary",
            "Search_DIV_HINT_Primary",
            "Search_INPUT_Primary",
            
            "Search_BUTTON_Search"
        ];
    }

    [Fact]
    public async Task HasErrorContent()
    {
        await Page.GetByTestId("Search_INPUT_Primary").FillAsync("INVALID");
        await Page.GetByTestId("Search_BUTTON_Search").ClickAsync();

        PageDataTestIdentifiers = [..PageDataTestIdentifiers, "Search_PARAGRAPH_Error_Primary"];
        
        await HasContent();

        await Expect(Page.Locator(".govuk-form-group.govuk-form-group--error")).ToBeVisibleAsync();
        await Expect(Page.Locator(".govuk-input.govuk-input--error")).ToBeVisibleAsync();
    }
}