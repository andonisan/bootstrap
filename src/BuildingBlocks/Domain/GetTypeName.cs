namespace BuildingBlocks.Domain;

public static class GetTypeName
{
    public static string? GetRequestName(this Type type) =>
        type.FullName?
            .Split('.')
            [^1]
            .Replace('+', '_');
}
