namespace Ontogony.SystemTests.Infrastructure;

/// <summary>Shared harness state for a test class instance.</summary>
public sealed class SystemTestFixture
{
    public HarnessOptions Options { get; } = HarnessOptions.FromEnvironment();
    public EvidenceWriter Evidence { get; }

    public SystemTestFixture() => Evidence = new EvidenceWriter(Options);

    public async Task WriteE2eEvidenceAsync(
        string testId,
        string scenarioId,
        string status,
        IReadOnlyDictionary<string, object?> assertions,
        IReadOnlyDictionary<string, object?>? correlation = null,
        object? response = null,
        object? events = null,
        IReadOnlyList<string>? services = null,
        int? statusCode = null,
        string? body = null,
        string? notes = null)
    {
        IReadOnlyDictionary<string, object?>? http = statusCode is null && body is null
            ? null
            : new Dictionary<string, object?>
            {
                ["statusCode"] = statusCode,
                ["body"] = body
            };

        var bundle = new E2eEvidenceBundle(
            E2eEvidenceBundle.SchemaV1,
            testId,
            scenarioId,
            DateTimeOffset.UtcNow,
            status,
            assertions,
            correlation,
            http,
            response,
            events,
            services ?? ["kanon", "conexus", "allagma"],
            notes);

        await Evidence.WriteBundleAsync(scenarioId, bundle);
    }
}
