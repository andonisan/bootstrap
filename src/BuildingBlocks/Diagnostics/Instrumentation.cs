using System.Diagnostics;

namespace BuildingBlocks.Diagnostics;

public static class Instrumentation
{
    public static readonly ActivitySource Source = new ActivitySource("core");
}
