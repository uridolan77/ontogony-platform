using System.ComponentModel.DataAnnotations;

namespace Ontogony.Http;

/// <summary>
/// Named-client options for Ontogony integration <see cref="HttpClient"/> registration.
/// </summary>
public sealed class HttpIntegrationOptions
{
    /// <summary>Whether outbound calls are disabled, faked, or use real HTTP.</summary>
    public IntegrationAdapterMode Mode { get; set; } = IntegrationAdapterMode.Disabled;

    /// <summary>Optional base URL for relative requests.</summary>
    public string? BaseUrl { get; set; }

    /// <summary>Per-request timeout (seconds).</summary>
    [Range(1, 600)]
    public int TimeoutSeconds { get; set; } = 30;

    /// <summary>Default headers applied to every request for this client.</summary>
    public Dictionary<string, string> DefaultHeaders { get; set; } = new();
}
