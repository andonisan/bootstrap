namespace FunctionalTests.Features;

internal static class ApiDefinition
{
    internal static class V1
    {
        internal static class Todo
        {
            public static string GetTodos() => $"/todos";

            public static string CreateTodo() => $"/todos";

            public static string GetTodo(Guid id) => $"/todos/{id}";

            public static string DeleteTodo(Guid id) => $"/todos/{id}";

            public static string UpdateTodo(Guid id) => $"/todos/{id}";

            public static string CompleteTodo(Guid id) => $"/todos/{id}/complete";
        }
    }

}
