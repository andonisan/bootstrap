using BuildingBlocks.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notifications.Diagnostics;
using Notifications.Infrastructure.Hubs;

namespace Notifications;

public static class NotificationsModule
{
    public static WebApplicationBuilder Install(WebApplicationBuilder builder)
    {
        builder.AddAssemblyServices(typeof(NotificationsModule).Assembly);

        builder.ConfigureMail();

        builder.Services.AddTransient<TodoCompletedEventConsumer>();
        builder.Services.AddTransient<TodoCreatedEventConsumer>();

        builder.Services.AddOpenTelemetry().WithTracing(x => x.AddSource(Instrumentation.ServiceName));

        return builder;
    }

    public static WebApplication Map(WebApplication app)
    {
        app.MapHub<TodoHub>("hubs/todo");

        return app;
    }

    private static void ConfigureMail(this WebApplicationBuilder applicationBuilder)
    {
        var mail = applicationBuilder.Configuration.GetConnectionString("mail");

        var smtpUri = new Uri(mail!);

        applicationBuilder.Services
            .AddFluentEmail("fromemail@test.test")
            .AddSmtpSender(smtpUri.Host, smtpUri.Port);
    }
}
