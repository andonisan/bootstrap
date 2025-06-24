using BuildingBlocks.Diagnostics;
using BuildingBlocks.Domain;
using Microsoft.Extensions.Diagnostics.Buffering;

namespace BuildingBlocks.CQRS.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(
    GlobalLogBuffer logBuffer,
    AppDiagnostics diagnostics) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        string? requestName = request.GetType().GetRequestName();

        diagnostics.LogHandlingRequest(requestName!);

        try
        {
            var response = await next();

            diagnostics.LogSuccessRequest(requestName!);

            return response;
        }
        catch (Exception ex)
        {
            diagnostics.LogErrorInRequest(requestName!, ex);
            
            logBuffer.Flush();
            
            throw;
        }
    }
}
