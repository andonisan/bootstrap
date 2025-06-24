using System.Text;
using System.Text.Json;

namespace E2ETests.Seedwork;

internal static class HttpClientExtensions
{
    internal static async Task<HttpResponseMessage> PostAsync(this HttpClient httpClient, string requestUri,
        object content) =>
        await httpClient.PostAsync(requestUri, CreateHttpContent(content));

    internal static async Task<HttpResponseMessage> PutAsync(this HttpClient httpClient, string requestUri,
        object content) =>
        await httpClient.PutAsync(requestUri, CreateHttpContent(content));

    private static HttpContent CreateHttpContent(object data)
    {
        var content = JsonSerializer.Serialize(data);
        return new StringContent(content, Encoding.Unicode, "application/json");
    }
}
