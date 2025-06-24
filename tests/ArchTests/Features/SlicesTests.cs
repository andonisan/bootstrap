using BuildingBlocks.CQRS;
using MediatR;

namespace ArchTests.Features;

public class SlicesTests
{
    [Fact]
    public void AllRequestsShouldBeUsedAsQueryOrCommand()
    {
        var result = AppTypes()
            .That()
            .ImplementInterface<IRequest>()
            .And()
            .AreClasses()
            .Or()
            .ImplementInterface(typeof(IRequest<>))
            .And()
            .AreClasses()
            .Should()
            .ImplementInterface(typeof(IQuery<>))
            .Or()
            .ImplementInterface<IBaseCommand>()
            .GetResult();


        result.IsSuccessful.ShouldBeTrue();
    }

    [Fact]
    public void AllSlicesNotHaveDependenciesBetweenSlices()
    {
        var result = AppTypes()
            .Slice()
            .ByNamespacePrefix(nameof(Todos.Application.Features))
            .Should()
            .NotHaveDependenciesBetweenSlices()
            .GetResult();

        result.IsSuccessful.ShouldBeTrue();
    }
}


