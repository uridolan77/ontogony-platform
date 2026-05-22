using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ontogony.SystemCompatibility;

public static class SystemCompatibilitySummaryWriter
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        WriteIndented = true
    };

    public static void WriteArtifacts(SystemCompatibilityGateResult result, string artifactDirectory)
    {
        Directory.CreateDirectory(artifactDirectory);
        var jsonPath = Path.Combine(artifactDirectory, "system-compatibility-summary.json");
        var mdPath = Path.Combine(artifactDirectory, "system-compatibility-summary.md");

        var document = new SystemCompatibilitySummaryDocument(
            result.Schema,
            result.Baseline,
            result.EvaluatedAtUtc,
            result.DevRoot,
            result.Passed,
            result.PassCount,
            result.FailCount,
            result.SkippedCount,
            result.Checks.Select(c => new SystemCompatibilitySummaryCheck(
                c.Id,
                c.Title,
                c.Status.ToString(),
                c.Detail)).ToList());

        File.WriteAllText(jsonPath, JsonSerializer.Serialize(document, JsonOptions));
        File.WriteAllText(mdPath, RenderMarkdown(result));
    }

    public static string RenderMarkdown(SystemCompatibilityGateResult result)
    {
        var sb = new StringBuilder();
        sb.AppendLine("# System compatibility summary");
        sb.AppendLine();
        sb.AppendLine($"| Field | Value |");
        sb.AppendLine($"| --- | --- |");
        sb.AppendLine($"| Schema | `{result.Schema}` |");
        sb.AppendLine($"| Baseline | `{result.Baseline}` |");
        sb.AppendLine($"| Evaluated (UTC) | `{result.EvaluatedAtUtc:O}` |");
        sb.AppendLine($"| Dev root | `{result.DevRoot}` |");
        sb.AppendLine($"| Verdict | **{(result.Passed ? "PASS" : "FAIL")}** |");
        sb.AppendLine($"| Checks | pass={result.PassCount}, fail={result.FailCount}, skipped={result.SkippedCount} |");
        sb.AppendLine();
        sb.AppendLine("## Checks");
        sb.AppendLine();
        sb.AppendLine("| Id | Status | Title | Detail |");
        sb.AppendLine("| --- | --- | --- | --- |");
        foreach (var check in result.Checks)
        {
            var detail = check.Detail.Replace("|", "/");
            sb.AppendLine($"| `{check.Id}` | {check.Status} | {check.Title} | {detail} |");
        }

        return sb.ToString();
    }

    private sealed record SystemCompatibilitySummaryDocument(
        string Schema,
        string Baseline,
        DateTimeOffset EvaluatedAtUtc,
        string DevRoot,
        bool Passed,
        int PassCount,
        int FailCount,
        int SkippedCount,
        IReadOnlyList<SystemCompatibilitySummaryCheck> Checks);

    private sealed record SystemCompatibilitySummaryCheck(
        string Id,
        string Title,
        string Status,
        string Detail);
}
