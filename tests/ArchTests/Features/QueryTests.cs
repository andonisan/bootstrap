using BuildingBlocks.CQRS;

namespace ArchTests.Features;

public class QueryTests
{
    [Fact]
    public void AllQueriesShouldFinishWithQuery()
    {
        var types = AppTypes()
            .That()
            .ImplementInterface(typeof(IQuery<>))
            .GetTypes();

        types.All(t => t.FullName!.EndsWith("Query", StringComparison.OrdinalIgnoreCase))
            .ShouldBeTrue();
    }
}
