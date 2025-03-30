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
}