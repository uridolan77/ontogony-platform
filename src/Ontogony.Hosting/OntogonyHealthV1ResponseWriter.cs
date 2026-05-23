using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Ontogony.Hosting.Contracts;

namespace Ontogony.Hosting;

/// <summary>Maps ASP.NET health reports to health.v1 and ready.v1 JSON payloads.</summary>
public static class OntogonyHealthV1ResponseWriter
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);

    public static HealthCheckOptions CreateLivenessOptions() =>
        new()
        {
            Predicate = static _ => false,
            ResponseWriter = WriteHealthV1Async,
        };

    public static HealthCheckOptions CreateReadinessOptions() =>
        new()
        {
            Predicate = static registration => registration.Tags.Contains("ready"),
            ResponseWriter = WriteReadyV1Async,
        };

    public static async Task WriteHealthV1Async(HttpContext context, HealthReport report)
    {
        var options = context.RequestServices.GetRequiredService<IOptions<OntogonyServiceDefaultsOptions>>().Value;
        var environment = context.RequestServices.GetRequiredService<IHostEnvironment>();
        var payload = BuildHealthV1(report, options, environment);
        context.Response.ContentType = "application/json; charset=utf-8";
        await context.Response.WriteAsync(JsonSerializer.Serialize(payload, SerializerOptions));
    }

    public static async Task WriteReadyV1Async(HttpContext context, HealthReport report)
    {
        var options = context.RequestServices.GetRequiredService<IOptions<OntogonyServiceDefaultsOptions>>().Value;
        var payload = BuildReadyV1(report, options);
        context.Response.ContentType = "application/json; charset=utf-8";
        context.Response.StatusCode = payload.Status switch
        {
            "ready" => StatusCodes.Status200OK,
            "degraded" => StatusCodes.Status200OK,
            _ => StatusCodes.Status503ServiceUnavailable,
        };
        await context.Response.WriteAsync(JsonSerializer.Serialize(payload, SerializerOptions));
    }

    internal static HealthV1Response BuildHealthV1(
        HealthReport report,
        OntogonyServiceDefaultsOptions options,
        IHostEnvironment environment)
    {
        var checkedAt = report.TotalDuration > TimeSpan.Zero
            ? DateTimeOffset.UtcNow
            : DateTimeOffset.UtcNow;

        var status = MapAggregateHealthStatus(report.Status);
        return new HealthV1Response(
            SchemaVersion: "health.v1",
            Status: status,
            Service: ResolveServiceId(options),
            ServiceDisplayName: ResolveServiceDisplayName(options),
            Version: options.ServiceVersion,
            Baseline: options.SystemBaseline,
            GitSha: options.GitSha,
            BuildTimeUtc: options.BuildTimeUtc,
            Environment: environment.EnvironmentName,
            InstanceId: options.InstanceId,
            CheckedAtUtc: checkedAt);
    }

    internal static ReadyV1Response BuildReadyV1(HealthReport report, OntogonyServiceDefaultsOptions options)
    {
        var checks = report.Entries
            .Select(entry => MapReadinessCheck(entry.Key, entry.Value))
            .OrderBy(static check => check.Id, StringComparer.Ordinal)
            .ToArray();

        var (status, failures, warnings) = AggregateReadyStatus(checks);
        return new ReadyV1Response(
            SchemaVersion: "ready.v1",
            Status: status,
            Service: ResolveServiceId(options),
            CheckedAtUtc: DateTimeOffset.UtcNow,
            Checks: checks,
            Summary: BuildReadySummary(status, checks),
            Warnings: warnings.Count > 0 ? warnings : null,
            Failures: failures.Count > 0 ? failures : null);
    }

    private static string ResolveServiceId(OntogonyServiceDefaultsOptions options)
    {
        if (!string.IsNullOrWhiteSpace(options.ServiceHealthId))
        {
            return options.ServiceHealthId.Trim();
        }

        var name = options.ServiceName.Trim();
        if (name.EndsWith("-api", StringComparison.OrdinalIgnoreCase))
        {
            return name[..^4];
        }

        return name;
    }

    private static string ResolveServiceDisplayName(OntogonyServiceDefaultsOptions options) =>
        string.IsNullOrWhiteSpace(options.ServiceDisplayName)
            ? ResolveServiceId(options)
            : options.ServiceDisplayName.Trim();

    private static string MapAggregateHealthStatus(HealthStatus status) =>
        status switch
        {
            HealthStatus.Healthy => "healthy",
            HealthStatus.Degraded => "degraded",
            HealthStatus.Unhealthy => "unhealthy",
            _ => "unknown",
        };

    private static ReadinessCheckV1 MapReadinessCheck(string registrationName, HealthReportEntry entry)
    {
        var tags = entry.Tags.ToArray();
        var id = ResolveCheckId(registrationName, tags);
        var severity = ResolveSeverity(tags);
        var checkStatus = MapCheckStatus(entry.Status);
        var detail = entry.Description
            ?? entry.Exception?.Message
            ?? (checkStatus switch
            {
                "ready" => "Check passed.",
                "warning" => "Check reported a warning.",
                "failed" => "Check failed.",
                _ => null,
            });

        return new ReadinessCheckV1(
            Id: id,
            Label: HumanizeCheckLabel(id),
            Status: checkStatus,
            Severity: severity,
            Detail: detail,
            LastCheckedAtUtc: DateTimeOffset.UtcNow);
    }

    private static string ResolveCheckId(string registrationName, IReadOnlyList<string> tags)
    {
        foreach (var tag in tags)
        {
            if (tag.StartsWith("check-id:", StringComparison.OrdinalIgnoreCase))
            {
                return tag["check-id:".Length..];
            }
        }

        return registrationName.Replace('_', '.', StringComparison.Ordinal);
    }

    private static string ResolveSeverity(IReadOnlyList<string> tags)
    {
        if (tags.Any(static t => string.Equals(t, "optional", StringComparison.OrdinalIgnoreCase)))
        {
            return "optional";
        }

        if (tags.Any(static t => string.Equals(t, "informational", StringComparison.OrdinalIgnoreCase)))
        {
            return "informational";
        }

        return "required";
    }

    private static string MapCheckStatus(HealthStatus status) =>
        status switch
        {
            HealthStatus.Healthy => "ready",
            HealthStatus.Degraded => "warning",
            HealthStatus.Unhealthy => "failed",
            _ => "unknown",
        };

    private static (string Status, List<string> Failures, List<string> Warnings) AggregateReadyStatus(
        IReadOnlyList<ReadinessCheckV1> checks)
    {
        var failures = new List<string>();
        var warnings = new List<string>();
        var requiredFailed = false;
        var requiredWarning = false;
        var optionalWarning = false;

        foreach (var check in checks)
        {
            if (check.Status is "failed")
            {
                var message = $"{check.Id}: {check.Detail ?? check.Label}";
                if (string.Equals(check.Severity, "required", StringComparison.Ordinal))
                {
                    requiredFailed = true;
                    failures.Add(message);
                }
                else
                {
                    optionalWarning = true;
                    warnings.Add(message);
                }
            }
            else if (check.Status is "warning")
            {
                var message = $"{check.Id}: {check.Detail ?? check.Label}";
                if (string.Equals(check.Severity, "required", StringComparison.Ordinal))
                {
                    requiredWarning = true;
                    warnings.Add(message);
                }
                else
                {
                    optionalWarning = true;
                    warnings.Add(message);
                }
            }
        }

        if (requiredFailed)
        {
            return ("not_ready", failures, warnings);
        }

        if (requiredWarning)
        {
            return ("degraded", failures, warnings);
        }

        if (optionalWarning)
        {
            return ("degraded", failures, warnings);
        }

        if (checks.Count == 0)
        {
            return ("ready", failures, warnings);
        }

        return ("ready", failures, warnings);
    }

    private static string? BuildReadySummary(string status, IReadOnlyList<ReadinessCheckV1> checks)
    {
        if (checks.Count == 0)
        {
            return "No readiness checks registered.";
        }

        var failing = checks.Where(static c =>
                c.Status is "failed" && string.Equals(c.Severity, "required", StringComparison.Ordinal))
            .Select(static c => c.Id)
            .ToArray();

        if (failing.Length > 0)
        {
            return $"Required checks not ready: {string.Join(", ", failing)}.";
        }

        return status switch
        {
            "ready" => "All required readiness checks passed.",
            "degraded" => "Service is usable with warnings on optional checks.",
            _ => null,
        };
    }

    private static string HumanizeCheckLabel(string id)
    {
        var lastSegment = id.Contains('.') ? id[(id.LastIndexOf('.') + 1)..] : id;
        return lastSegment.Replace('_', ' ', StringComparison.Ordinal);
    }
}
