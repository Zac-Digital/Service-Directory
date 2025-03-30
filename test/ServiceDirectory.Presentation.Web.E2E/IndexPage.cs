namespace ServiceDirectory.Presentation.Web.E2E;

public class IndexPage : BasePageTest
{
    public IndexPage()
    {
        PageRelativeUrl = "/";
        PageTitle = "Start Page - GOV.UK";
        PageDataTestIdentifiers =
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
    }
}