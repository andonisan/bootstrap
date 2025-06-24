using AspireHost.Integrations;
using static AspireHost.Constants;

var builder = DistributedApplication.CreateBuilder(args);

var sqlServer = builder.AddAzurePostgresFlexibleServer("sql");

var sql = sqlServer.AddDatabase(Database);

var cache = builder.AddRedis("cache");

var mail = builder.AddMailDev("mail")
    .WithExternalHttpEndpoints();

builder.AddProject<Projects.DbService>("sql-service")
    .WithHttpCommand("/reset-db", "Reset Database", commandOptions: new HttpCommandOptions
    {
        IconName = "DatabaseLightning",
    })
    .WithReference(sql)
    .WaitFor(sql);

var api = builder.AddProject<Projects.Api>(Api)
    .WithReference(sql)
    .WithReference(cache)
    .WithReference(mail)
    .WithExternalHttpEndpoints()
    .WaitFor(sql);

builder.AddViteApp("front", "../spa")
    .WithNpmPackageInstallation()
    .WithReference(api)
    .WithOtlpExporter()
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

if (builder.ExecutionContext.IsRunMode)
{
    sqlServer
        .RunAsContainer(c =>
            c.WithLifetime(ContainerLifetime.Persistent)
                .WithPgWeb());

    cache.WithLifetime(ContainerLifetime.Persistent)
        .WithRedisInsight();

    var seq = builder.AddSeq("seq");

    api.WithReference(seq)
        .WithUrls(ctx =>
        {
            ctx.Urls.Add(new ResourceUrlAnnotation
            {
                Url = $"{api.GetEndpoint("https").Url}/swagger",
                DisplayText = "Swagger UI",
            });
            ctx.Urls.Add(new ResourceUrlAnnotation
            {
                Url = $"{api.GetEndpoint("https").Url}/cap",
                DisplayText = "Cap UI",
            });
        });
}

builder.Build().Run();
