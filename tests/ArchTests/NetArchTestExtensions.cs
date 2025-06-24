namespace ArchTests;

internal static class NetArchTestExtensions
{
    public static Types AppTypes() => Types.InAssembly(typeof(Todos.TodosModule).Assembly);
}
