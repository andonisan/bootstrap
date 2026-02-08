using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace Todos.Diagnostics;

internal static class Instrumentation
{
    public const string ServiceName = "todos";

    private static readonly Meter Meter = new(ServiceName);

    public static readonly Counter<long> Created = Meter.CreateCounter<long>("todos.created");
    public static readonly Counter<long> Completed = Meter.CreateCounter<long>("todos.completed");
    public static readonly ActivitySource Source = new(ServiceName);
}
