using System.Reflection;
using BuildingBlocks.Domain;
using BuildingBlocks.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BuildingBlocks.DependencyInjection;

public static class AddFeatureModules
{
    public static IServiceCollection AddEndpoints(this IServiceCollection services, Assembly assembly)
    {
        var serviceDescriptors = assembly
             .DefinedTypes
             .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                            type.IsAssignableTo(typeof(IFeatureModule)))
             .Select(type => ServiceDescriptor.Transient(typeof(IFeatureModule), type))
             .ToArray();

        services.TryAddEnumerable(serviceDescriptors);

        return services;
    }

    public static IApplicationBuilder MapEndpoints(this WebApplication app, RouteGroupBuilder? routeGroupBuilder = null)
    {
        var endpoints = app.Services.GetRequiredService<IEnumerable<IFeatureModule>>();

        IEndpointRouteBuilder builder = routeGroupBuilder is null ? app : routeGroupBuilder;

        foreach (var endpoint in endpoints)
        {
            endpoint.AddRoutes(builder);
        }

        return app;
    }
}
