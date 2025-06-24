using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Savorboard.CAP.InMemoryMessageQueue;

namespace Api;

internal static class ServiceCollectionExtensions
{
    public static void AddIntegrationCommunucation(this WebApplicationBuilder builder)
    {
        var sql = builder.Configuration.GetConnectionString("TodoAppDb");

        builder.Services.AddCap(options =>
        {
            options.UseInMemoryMessageQueue();

            options.UsePostgreSql(sql!);

            options.UseDashboard();
        });
    }

    public static WebApplicationBuilder AddCustomSeqEndpoint(this WebApplicationBuilder builder)
    {
        if (builder.Configuration.GetConnectionString("seq") != null)
        {
            builder.AddSeqEndpoint("seq");
        }

        return builder;
    }
}
