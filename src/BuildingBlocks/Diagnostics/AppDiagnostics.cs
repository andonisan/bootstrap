namespace BuildingBlocks.Diagnostics;

public partial class AppDiagnostics
{
    private readonly ILogger _logger;

    public AppDiagnostics(ILoggerFactory loggerFactory)
    {
        _ = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        _logger = loggerFactory.CreateLogger(Instrumentation.Source.Name);
    }

    [LoggerMessage(0, LogLevel.Information, "----- Processing {requestName}")]
    public partial void LogHandlingRequest(string requestName);

    [LoggerMessage(1, LogLevel.Information, "----- Completed {requestName}")]
    public partial void LogSuccessRequest(string requestName);

    [LoggerMessage(2, LogLevel.Error, "----- Completed {requestName} with error")]
    public partial void LogErrorInRequest(string requestName, Exception ex);


    [LoggerMessage(
        EventName = nameof(GettingOrderRequest),
        Level = LogLevel.Information,
        Message = "Getting order request.")]
    public partial void GettingOrderRequest();
}
