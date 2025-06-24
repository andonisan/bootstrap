using Notifications;

namespace ArchTests.Modules;

public class ModulesTest
{
    [Fact]
    public void TodosModuleDoesNotHaveDependencyOnOtherModules()
    {
        var otherModules = new[]
        {
            typeof(NotificationsModule).Namespace,
        };

        var result = AppTypes()
            .ShouldNot()
            .HaveDependencyOnAll(otherModules)
            .GetResult();

        result.IsSuccessful.ShouldBeTrue();
    }
}
