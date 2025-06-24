using System.Diagnostics;
using Aspirant.Hosting.Testing;
using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;

namespace E2ETests.Seedwork;

public class AppHostFixture : IAsyncDisposable
{

    private readonly Task<DistributedApplication> _appInitializer;

    public Resource Web { get; }

    public static string ActivitySourceName => "E2ETests";

    public static ActivitySource ActivitySource { get; } = new(ActivitySourceName);

    public AppHostFixture()
    {
        Web = new Resource("front", this);
        _appInitializer = InitializeAsync();
    }

    private async Task<DistributedApplication> InitializeAsync()
    {
        using var activityScope = ActivitySource.StartActivity("Tear_Up");

        var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.AspireHost>();

        appHost.FixContentRoot();
        appHost.Services.AddResourceWatching();
        appHost.WithRandomVolumeNames();
        appHost.WithContainersLifetime(ContainerLifetime.Session);

        appHost.Environment.EnvironmentName = "E2E";

        var app = await appHost.BuildAsync();

        await app.StartAsync(waitForResourcesToStart: true);

        return app;
    }

    public async ValueTask DisposeAsync()
    {
        using var activityScope = ActivitySource.StartActivity("Tear_Down");
        var app = await _appInitializer;
        await app.StopAsync();
        await app.DisposeAsync();

    }

    public class Resource(string name, AppHostFixture owner)
    {
        public async Task<HttpClient> CreateHttpClientAsync()
            => (await owner._appInitializer).CreateHttpClient(name);

        public async Task<string> ResolveUrlAsync(string relativeUrl)
        {
            var app = await owner._appInitializer;
            var baseUri = app.GetEndpoint(name);
            return new Uri(baseUri, relativeUrl).ToString();
        }
    }
}
