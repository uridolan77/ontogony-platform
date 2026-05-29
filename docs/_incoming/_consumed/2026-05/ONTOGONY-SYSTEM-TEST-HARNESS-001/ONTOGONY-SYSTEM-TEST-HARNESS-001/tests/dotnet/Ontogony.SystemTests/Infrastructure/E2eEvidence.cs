namespace Ontogony.SystemTests.Infrastructure;

public sealed record E2eEvidenceBundle(
    string Schema,
    string TestId,
    string ScenarioId,
    DateTimeOffset CapturedAtUtc,
    string Status,
    IReadOnlyDictionary<string, object?> Assertions,
    IReadOnlyDictionary<string, object?>? Correlation = null,
    IReadOnlyDictionary<string, object?>? Http = null,
    object? Response = null,
    object? Events = null,
    IReadOnlyList<string>? Services = null,
    string? Notes = null)
{
    public const string SchemaV1 = "ontogony-system-test-harness-evidence-v1";
}
