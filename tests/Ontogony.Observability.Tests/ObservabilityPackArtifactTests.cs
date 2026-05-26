using System.Text.Json;
using Xunit;

namespace Ontogony.Observability.Tests;

public sealed class ObservabilityPackArtifactTests
{
    private static string RepoRoot =>
        Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", ".."));

    [Fact]
    public void Grafana_dashboard_starter_json_parses_with_required_uid()
    {
        var path = Path.Combine(
            RepoRoot,
            "docs",
            "observability",
            "dashboards",
            "grafana-dashboard-starter.json");

        Assert.True(File.Exists(path), $"Missing dashboard: {path}");

        using var doc = JsonDocument.Parse(File.ReadAllText(path));
        var root = doc.RootElement;
        Assert.Equal("ontogony-observability-mechanics", root.GetProperty("uid").GetString());
        Assert.True(root.GetProperty("panels").GetArrayLength() > 0);
    }

    [Fact]
    public void Prometheus_alert_rules_define_required_service_groups()
    {
        var path = Path.Combine(
            RepoRoot,
            "docs",
            "observability",
            "alerts",
            "alerts.prometheus.rules.yml");

        Assert.True(File.Exists(path), $"Missing alerts: {path}");

        var yaml = File.ReadAllText(path);
        foreach (var group in new[]
                 {
                     "ontogony-platform-mechanics",
                     "allagma-mechanics",
                     "conexus-mechanics",
                     "kanon-mechanics",
                 })
        {
            Assert.Contains($"name: {group}", yaml, StringComparison.Ordinal);
        }

        Assert.Contains("alert:", yaml, StringComparison.Ordinal);
    }
}
