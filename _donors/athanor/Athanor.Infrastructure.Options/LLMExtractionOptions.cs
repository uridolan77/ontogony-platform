namespace Athanor.Infrastructure.Options;

/// <summary>
/// Configuration for backend LLM extraction. Live OpenAI-compatible traffic is opt-in (PR99).
/// </summary>
public sealed class LLMExtractionOptions
{
    public const string SectionName = "LLMExtraction";

    /// <summary>
    /// When <c>true</c>, exposes <c>POST /source-versions/{{id}}/generated-extraction-jobs</c> (PR93). Must remain <c>false</c> in normal deployments until explicitly enabled.
    /// </summary>
    public bool EnableGeneratedExtractionEndpoint { get; init; }

    /// <summary><c>Fake</c> (default) or <c>OpenAICompatible</c> (PR99, requires explicit enablement).</summary>
    public string ProviderKind { get; init; } = "Fake";

    public string FakeModelName { get; init; } = "fake-llm-extractor-v1";

    /// <summary>HTTPS base URL for the OpenAI-compatible API (include <c>/v1</c> path segment).</summary>
    public string? EndpointBaseUrl { get; init; }

    /// <summary>Model or deployment name for chat completions.</summary>
    public string? ModelName { get; init; }

    /// <summary>Environment variable **name** that holds the API key; value is never bound into options.</summary>
    public string? SecretEnvVarName { get; init; }

    /// <summary><c>bearer</c> (Authorization: Bearer …) or <c>api-key-header</c> (<c>api-key</c> header).</summary>
    public string AuthHeaderStyle { get; init; } = "bearer";

    /// <summary>Must be <c>true</c> to register outbound live provider calls.</summary>
    public bool EnableLiveProvider { get; init; }

    /// <summary>Production/staging hosts must set this when using a live provider.</summary>
    public bool AllowLiveProviderInNonDevelopment { get; init; }

    /// <summary>Allows <c>http://</c> endpoints only in Development when set.</summary>
    public bool AllowHttpEndpointInDevelopment { get; init; }

    public TimeSpan RequestTimeout { get; init; } = TimeSpan.FromSeconds(60);

    public int MaxRetryAttempts { get; init; }

    public int? MaxOutputTokensBudget { get; init; }

    /// <summary>PR98: rejects generated extraction when source raw text exceeds this length.</summary>
    public int MaxSourceTextChars { get; init; } = 80_000;

    /// <summary>PR98: max chunk rows loaded when <c>IncludeChunks</c> is true.</summary>
    public int MaxChunksPerGeneratedExtraction { get; init; } = 40;

    /// <summary>PR98: max characters per chunk text passed to the provider.</summary>
    public int MaxChunkTextChars { get; init; } = 4_000;

    /// <summary>PR98: upper bound for <c>GenerateExtractionJobRequest.MaxProposals</c> (controller validates).</summary>
    public int MaxGeneratedExtractionProposals { get; init; } = 20;

    /// <summary>PR98: reserved for future persistence of rejected attempts; no effect yet.</summary>
    public bool RecordFailedGeneratedExtractionAttempts { get; init; }
}
