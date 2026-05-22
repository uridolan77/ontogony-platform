using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ontogony.SystemCompatibility;

public static class ConsumerConformanceSummaryWriter
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        WriteIndented = true
    };

    public static void WriteArtifacts(
        ConsumerConformanceResult result,
        string artifactDirectory,
        string fileBaseName = "summary")
    {
        Directory.CreateDirectory(artifactDirectory);
        var jsonPath = Path.Combine(artifactDirectory, $"{fileBaseName}.json");
        var mdPath = Path.Combine(artifactDirectory, $"{fileBaseName}.md");

        var document = new ConsumerConformanceSummaryDocument(
            result.Schema,
            result.Baseline,
            result.EvaluatedAtUtc,
            result.DevRoot,
            result.Verdict,
            result.Passed,
            result.StrictMode,
            result.Consumers.Select(c => new ConsumerConformanceSummaryRow(
                c.Consumer,
                c.Verdict,
                c.PassCount,
                c.WarnCount,
                c.FailCount,
                c.SkippedCount,
                c.Notes,
                c.Proofs.Select(p => new ConsumerConformanceProofRow(
                    p.CheckId,
                    p.Status,
                    p.Title,
                    p.Detail)).ToList())).ToList());

        File.WriteAllText(jsonPath, JsonSerializer.Serialize(document, JsonOptions));
        File.WriteAllText(mdPath, RenderMarkdown(result));
    }

    public static string RenderMarkdown(ConsumerConformanceResult result)
    {
        var sb = new StringBuilder();
        sb.AppendLine("# Consumer conformance summary");
        sb.AppendLine();
        sb.AppendLine("| Field | Value |");
        sb.AppendLine("| --- | --- |");
        sb.AppendLine($"| Schema | `{result.Schema}` |");
        sb.AppendLine($"| Baseline | `{result.Baseline}` |");
        sb.AppendLine($"| Evaluated (UTC) | `{result.EvaluatedAtUtc:O}` |");
        sb.AppendLine($"| Dev root | `{result.DevRoot}` |");
        sb.AppendLine($"| Mode | {(result.StrictMode ? "**release/strict**" : "development")} |");
        sb.AppendLine($"| Verdict | **{result.Verdict.ToUpperInvariant()}** |");
        sb.AppendLine();
        sb.AppendLine("## Consumers");
        sb.AppendLine();
        sb.AppendLine("| Consumer | Verdict | pass | warn | fail | skipped |");
        sb.AppendLine("| --- | --- | ---: | ---: | ---: | ---: |");
        foreach (var consumer in result.Consumers)
        {
            sb.AppendLine(
                $"| {consumer.Consumer} | **{consumer.Verdict}** | {consumer.PassCount} | {consumer.WarnCount} | {consumer.FailCount} | {consumer.SkippedCount} |");
        }

        foreach (var consumer in result.Consumers)
        {
            sb.AppendLine();
            sb.AppendLine($"### {consumer.Consumer}");
            sb.AppendLine();
            if (consumer.Notes.Count > 0)
            {
                foreach (var note in consumer.Notes)
                {
                    sb.AppendLine($"- {note}");
                }

                sb.AppendLine();
            }

            sb.AppendLine("| Check | Status | Title |");
            sb.AppendLine("| --- | --- | --- |");
            foreach (var proof in consumer.Proofs)
            {
                sb.AppendLine($"| `{proof.CheckId}` | {proof.Status} | {proof.Title} |");
            }
        }

        return sb.ToString();
    }

    private sealed record ConsumerConformanceSummaryDocument(
        string Schema,
        string Baseline,
        DateTimeOffset EvaluatedAtUtc,
        string DevRoot,
        string Verdict,
        bool Passed,
        bool StrictMode,
        IReadOnlyList<ConsumerConformanceSummaryRow> Consumers);

    private sealed record ConsumerConformanceSummaryRow(
        string Consumer,
        string Verdict,
        int PassCount,
        int WarnCount,
        int FailCount,
        int SkippedCount,
        IReadOnlyList<string> Notes,
        IReadOnlyList<ConsumerConformanceProofRow> Proofs);

    private sealed record ConsumerConformanceProofRow(
        string CheckId,
        string Status,
        string Title,
        string Detail);
}
