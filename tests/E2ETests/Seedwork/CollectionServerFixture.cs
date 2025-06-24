namespace E2ETests.Seedwork;

[CollectionDefinition(nameof(CollectionServerFixture))]
public class CollectionServerFixture
    : ICollectionFixture<AppHostFixture>
{
}
