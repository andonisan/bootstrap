using System.Diagnostics.CodeAnalysis;

namespace AspireHost.Integrations;

internal sealed class MailDevResource(string name) : ContainerResource(name), IResourceWithConnectionString
{
    internal const string SmtpEndpointName = "smtp";
    internal const string HttpEndpointName = "http";

    [field: AllowNull, MaybeNull]
    private EndpointReference SmtpEndpoint =>
        field ??= new(this, SmtpEndpointName);

    public ReferenceExpression ConnectionStringExpression =>
        ReferenceExpression.Create(
            $"smtp://{SmtpEndpoint.Property(EndpointProperty.Host)}:{SmtpEndpoint.Property(EndpointProperty.Port)}"
        );
}

internal static class MailDevResourceBuilderExtensions
{
    public static IResourceBuilder<MailDevResource> AddMailDev(
        this IDistributedApplicationBuilder builder,
        string name,
        int? httpPort = null,
        int? smtpPort = null)
    {
        var resource = new MailDevResource(name);

        return builder.AddResource(resource)
                      .WithImage(MailDevContainerImageTags.Image)
                      .WithImageRegistry(MailDevContainerImageTags.Registry)
                      .WithImageTag(MailDevContainerImageTags.Tag)
                      .WithHttpEndpoint(
                          targetPort: 1080,
                          port: httpPort,
                          name: MailDevResource.HttpEndpointName)
                      .WithEndpoint(
                          targetPort: 1025,
                          port: smtpPort,
                          name: MailDevResource.SmtpEndpointName);
    }
}

internal static class MailDevContainerImageTags
{
    internal const string Registry = "docker.io";

    internal const string Image = "maildev/maildev";

    internal const string Tag = "2.0.2";
}
