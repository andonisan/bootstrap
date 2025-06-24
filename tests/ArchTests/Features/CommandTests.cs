using BuildingBlocks.CQRS;



namespace ArchTests.Features;

public class CommandTests
{
    [Fact]
    public void AllCommandsShouldFinishWithCommand()
    {
        var types = AppTypes()
            .That()
            .ImplementInterface<ICommand>()
            .Or()
            .ImplementInterface(typeof(ICommand<>))
            .GetTypes();

        types.All(t => t.FullName!.EndsWith("Command", StringComparison.OrdinalIgnoreCase))
            .ShouldBeTrue();
    }
}
