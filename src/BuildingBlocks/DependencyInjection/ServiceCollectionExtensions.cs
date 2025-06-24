using System.Reflection;
using BuildingBlocks.Cache;
using BuildingBlocks.CQRS.Behaviors;
using BuildingBlocks.Diagnostics;
using BuildingBlocks.Domain;
using BuildingBlocks.DomainEvents;
using Hellang.Middleware.ProblemDetails;
using Microsoft.Extensions.DependencyInjection;
using ServiceDefaults;

namespace BuildingBlocks.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static WebApplicationBuilder AddApiServices(this WebApplicationBuilder builder)
    {
        builder.AddServiceDefaults();

        builder.Services
            .AddControllers();

        builder.Services
            .AddCustomProblemDetails()
            .AddSingleton<AppDiagnostics>()
            .AddDomainEvents()
            .AddBehaviors()
            .AddOpenApi();

        builder.Services.AddOpenTelemetry().WithTracing(t =>
            t.AddSource(Instrumentation.Source.Name));

        builder.AddCaching();

        return builder;
    }

    public static WebApplicationBuilder AddAssemblyServices(this WebApplicationBuilder app, Assembly assembly)
    {
        app.Services
            .AddValidatorsFromAssembly(assembly)
            .AddMediatR(x => x.RegisterServicesFromAssembly(assembly))
            .AddEndpoints(assembly);

        return app;
    }

    private static IServiceCollection AddOpenApi(this IServiceCollection services) =>
        services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen(options => options.CustomSchemaIds(y => y.GetRequestName()));

    private static IServiceCollection AddBehaviors(this IServiceCollection services) =>
        services
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(ActivityBehavior<,>))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    
    private static IServiceCollection AddCustomProblemDetails(this IServiceCollection services)
    {
        services.AddProblemDetails(setup =>
        {
            setup.Map<ArgumentException>(exception =>
                new StatusCodeProblemDetails(StatusCodes.Status409Conflict)
                {
                    Detail = exception.Message
                });
            setup.Map<InvalidOperationException>(exception =>
                new StatusCodeProblemDetails(StatusCodes.Status409Conflict)
                {
                    Detail = exception.Message
                });
            setup.Map<ValidationException>(exception =>
                new StatusCodeProblemDetails(StatusCodes.Status409Conflict)
                {
                    Detail = exception.Message
                });
            setup.MapToStatusCode<NotImplementedException>(StatusCodes.Status501NotImplemented);
            setup.MapToStatusCode<HttpRequestException>(StatusCodes.Status503ServiceUnavailable);
            setup.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);
        });

        return services;
    }
}
