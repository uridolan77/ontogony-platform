using System.Text.Json;

namespace Ontogony.SystemTests.Infrastructure;

public sealed class EvidenceWriter
{
    private readonly HarnessOptions _options;
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web) { WriteIndented = true };

    public EvidenceWriter(HarnessOptions options) => _options = options;

    public async Task WriteAsync(string scenarioId, object evidence)
    {
        Directory.CreateDirectory(_options.EvidenceDir);
        var path = Path.Combine(_options.EvidenceDir, $"{Sanitize(scenarioId)}.json");
        await File.WriteAllTextAsync(path, JsonSerializer.Serialize(evidence, JsonOptions));
    }

    public Task WriteBundleAsync(string scenarioId, E2eEvidenceBundle bundle) =>
        WriteAsync(scenarioId, bundle);

    private static string Sanitize(string value)
    {
        foreach (var c in Path.GetInvalidFileNameChars()) value = value.Replace(c, '-');
        return value;
    }
}
