using E2ETests.Seedwork;

namespace E2ETests;

[Collection(nameof(CollectionServerFixture))]
public class HomeTest(AppHostFixture app) : PlaywrightTestBase
{

    [Fact]
    public async Task HasPageTitle()
    {
        string url = await app.Web.ResolveUrlAsync("/");
        await Page.GotoAsync(url);
        await Expect(Page).ToHaveTitleAsync("TodoApp");
    }
}
