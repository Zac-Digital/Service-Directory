using System.Text.RegularExpressions;
using Microsoft.Playwright;

namespace ServiceDirectory.Presentation.Web.E2E;

public partial class ResultsPage : BasePageTest
{
    private const string Path = "/Results?Postcode=SW1A%201AA&Latitude=51.501009&Longitude=-0.141588&CurrentPage=";
    private const string PathEmpty = "/Results?Postcode=SW1A%201AA&Latitude=0.00&Longitude=0.00&CurrentPage=1";

    public ResultsPage()
    {
        PageRelativeUrl = $"{Path}1";
        PageTitle = "";
        PageDataTestIdentifiers =
        [
            "Results_HEADER_Primary",
            "Results_LIST_Primary",
            "Results_NAVIGATION_Primary"
        ];
    }

    [Fact]
    protected override async Task HasTitle() => await Expect(Page).ToHaveTitleAsync(PageTitleRegex());

    [Fact]
    public async Task WhenResultsAreFound_NotFoundPage_IsNotVisible()
    {
        List<string> notFoundPageDataTestIdentifiers =
        [
            "Results_HEADER_No-Results",
            "Results_PARAGRAPH_No-Results_ExampleOne",
            "Results_PARAGRAPH_No-Results_ExampleTwo",
            "Results_PARAGRAPH_No-Results_ExampleCallToAction"
        ];

        foreach (string notFoundPageDataTestIdentifier in notFoundPageDataTestIdentifiers)
        {
            await Expect(Page.GetByTestId(notFoundPageDataTestIdentifier)).Not.ToBeVisibleAsync();
        }
    }

    [Fact]
    public async Task HasListContent()
    {
        PageDataTestIdentifiers =
        [
            "Results_LIST_Secondary",
            "Results_HEADER_Service-Name",
            "Results_BODY_Service-Cost",
            "Results_BODY_Service-OpensAt",
            "Results_BODY_Service-ClosesAt",
            "Results_BODY_Service-DaysOpen"
        ];

        foreach (string pageDataTestIdentifier in PageDataTestIdentifiers)
        {
            ILocator pageElementLocator = Page.GetByTestId(pageDataTestIdentifier);

            await Expect(pageElementLocator).ToHaveCountAsync(10);

            for (int i = 0; i < await pageElementLocator.CountAsync(); i++)
            {
                await Expect(pageElementLocator.Nth(i)).ToBeVisibleAsync();
            }
        }
    }

    [Fact]
    public async Task FirstPage_HasOnly_Next_Navigation()
    {
        await Expect(Page.GetByTestId("Results_NAVIGATION_Next-Page")).ToBeVisibleAsync();
        await Expect(Page.GetByTestId("Results_NAVIGATION_Previous-Page")).Not.ToBeVisibleAsync();
    }

    [Fact]
    public async Task NthPage_Has_Previous_And_Next_Navigation()
    {
        await Page.GetByTestId("Results_NAVIGATION_LINK_Next-Page").ClickAsync();
        await Expect(Page.GetByTestId("Results_NAVIGATION_Next-Page")).ToBeVisibleAsync();
        await Expect(Page.GetByTestId("Results_NAVIGATION_Previous-Page")).ToBeVisibleAsync();
    }

    [Fact]
    public async Task LastPage_HasOnly_Previous_Navigation()
    {
        int lastPageNumber = int.Parse((await Page.TitleAsync()).Split(' ')[6]);

        await Page.GotoAsync(BaseUrl + $"{Path}{lastPageNumber}");

        await Expect(Page.GetByTestId("Results_NAVIGATION_Previous-Page")).ToBeVisibleAsync();
        await Expect(Page.GetByTestId("Results_NAVIGATION_Next-Page")).Not.ToBeVisibleAsync();
    }

    [Fact]
    public async Task When_NoResultsFound_NoResultsPage_IsVisible()
    {
        await Page.GotoAsync(BaseUrl + PathEmpty);

        PageDataTestIdentifiers =
        [
            ..PageDataTestIdentifiers,
            "Results_LIST_Secondary",
            "Results_HEADER_Service-Name",
            "Results_BODY_Service-Cost",
            "Results_BODY_Service-OpensAt",
            "Results_BODY_Service-ClosesAt",
            "Results_BODY_Service-DaysOpen"
        ];

        foreach (string pageDataTestIdentifier in PageDataTestIdentifiers)
        {
            await Expect(Page.GetByTestId(pageDataTestIdentifier)).ToHaveCountAsync(0);
        }

        List<string> notFoundPageDataTestIdentifiers =
        [
            "Results_HEADER_No-Results",
            "Results_PARAGRAPH_No-Results_ExampleOne",
            "Results_PARAGRAPH_No-Results_ExampleTwo",
            "Results_PARAGRAPH_No-Results_ExampleCallToAction"
        ];

        foreach (string notFoundPageDataTestIdentifier in notFoundPageDataTestIdentifiers)
        {
            await Expect(Page.GetByTestId(notFoundPageDataTestIdentifier)).ToBeVisibleAsync();
        }
    }

    [GeneratedRegex(@"^Search Results - Page \d+ of \d+ - GOV\.UK$")]
    private static partial Regex PageTitleRegex();
}