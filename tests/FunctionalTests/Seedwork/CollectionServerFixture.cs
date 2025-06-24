namespace FunctionalTests.Seedwork;

[CollectionDefinition(nameof(CollectionServerFixture))]
public class CollectionServerFixture
    : ICollectionFixture<ApiServiceFixture>
{
}
