using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Ontogony.SystemTests.Infrastructure;

public sealed class TestHttp : IDisposable
{
    private readonly HttpClient _client = new() { Timeout = TimeSpan.FromSeconds(120) };
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web) { WriteIndented = true };

    public async Task<HttpResponseMessage> GetAsync(
        string baseUrl,
        string path,
        string scenarioId,
        string? bearer = null,
        string? apiKey = null,
        IReadOnlyDictionary<string, string>? extraHeaders = null)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, Combine(baseUrl, path));
        AddAuth(request, bearer, apiKey);
        Correlation.AddStandardHeaders(request, scenarioId, extra: extraHeaders);
        return await _client.SendAsync(request);
    }

    public Task<HttpResponseMessage> PostJsonAsync(
        string baseUrl,
        string path,
        object payload,
        string scenarioId,
        string? bearer = null,
        string? apiKey = null,
        string? idempotencyKey = null,
        string? traceId = null,
        IReadOnlyDictionary<string, string>? extraHeaders = null) =>
        SendJsonAsync(HttpMethod.Post, baseUrl, path, payload, scenarioId, bearer, apiKey, idempotencyKey, traceId, extraHeaders);

    public Task<HttpResponseMessage> PutJsonAsync(
        string baseUrl,
        string path,
        object payload,
        string scenarioId,
        string? bearer = null,
        string? apiKey = null,
        IReadOnlyDictionary<string, string>? extraHeaders = null) =>
        SendJsonAsync(HttpMethod.Put, baseUrl, path, payload, scenarioId, bearer, apiKey, null, null, extraHeaders);

    public Task<HttpResponseMessage> PostEmptyAsync(
        string baseUrl,
        string path,
        string scenarioId,
        string? bearer = null,
        string? idempotencyKey = null) =>
        SendAsync(HttpMethod.Post, baseUrl, path, scenarioId, bearer, null, idempotencyKey);

    public async Task<HttpResponseMessage> SendAsync(
        HttpMethod method,
        string baseUrl,
        string path,
        string scenarioId,
        string? bearer = null,
        string? apiKey = null,
        string? idempotencyKey = null,
        string? traceId = null,
        IReadOnlyDictionary<string, string>? extraHeaders = null)
    {
        using var request = new HttpRequestMessage(method, Combine(baseUrl, path));
        AddAuth(request, bearer, apiKey);
        Correlation.AddStandardHeaders(request, scenarioId, idempotencyKey, traceId, extraHeaders);
        return await _client.SendAsync(request);
    }

    private async Task<HttpResponseMessage> SendJsonAsync(
        HttpMethod method,
        string baseUrl,
        string path,
        object payload,
        string scenarioId,
        string? bearer,
        string? apiKey,
        string? idempotencyKey,
        string? traceId,
        IReadOnlyDictionary<string, string>? extraHeaders)
    {
        using var request = new HttpRequestMessage(method, Combine(baseUrl, path));
        AddAuth(request, bearer, apiKey);
        Correlation.AddStandardHeaders(request, scenarioId, idempotencyKey, traceId, extraHeaders);
        var json = JsonSerializer.Serialize(payload, JsonOptions);
        request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        return await _client.SendAsync(request);
    }

    public static async Task<JsonDocument?> TryReadJsonAsync(HttpResponseMessage response)
    {
        var text = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrWhiteSpace(text))
        {
            return null;
        }

        try
        {
            return JsonDocument.Parse(text);
        }
        catch
        {
            return null;
        }
    }

    private static void AddAuth(HttpRequestMessage request, string? bearer, string? apiKey)
    {
        if (!string.IsNullOrWhiteSpace(bearer))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearer);
        }

        if (!string.IsNullOrWhiteSpace(apiKey))
        {
            request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {apiKey}");
            request.Headers.TryAddWithoutValidation("X-API-Key", apiKey);
        }
    }

    private static string Combine(string baseUrl, string path) =>
        baseUrl.TrimEnd('/') + "/" + path.TrimStart('/');

    public void Dispose() => _client.Dispose();
}
