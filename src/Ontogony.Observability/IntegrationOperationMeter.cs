using System.Diagnostics;
using Microsoft.Extensions.Options;

namespace Ontogony.Observability;

/// <summary>
/// Default <see cref="IIntegrationOperationMeter"/> backed by <see cref="OntogonyDiagnostics"/> instruments.
/// </summary>
public sealed class IntegrationOperationMeter : IIntegrationOperationMeter
{
    private readonly string _sourceService;

    /// <summary>Creates a meter using <see cref="OntogonyObservabilityOptions.ServiceName"/> as <see cref="IntegrationMetricDimensions.SourceService"/>.</summary>
    public IntegrationOperationMeter(IOptions<OntogonyObservabilityOptions> options)
    {
        ArgumentNullException.ThrowIfNull(options);
        _sourceService = options.Value.ServiceName;
    }

    /// <inheritdoc />
    public IDisposable StartCall(
        string targetService,
        string operation,
        IReadOnlyDictionary<string, string>? dimensions = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(targetService);
        ArgumentException.ThrowIfNullOrWhiteSpace(operation);

        var activity = OntogonyDiagnostics.ActivitySource.StartActivity(
            $"integration.{targetService}.{operation}",
            ActivityKind.Client);

        if (activity is not null)
        {
            activity.SetTag(IntegrationMetricDimensions.SourceService, _sourceService);
            activity.SetTag(IntegrationMetricDimensions.TargetService, targetService);
            activity.SetTag(IntegrationMetricDimensions.Operation, operation);
            ApplyActivityTags(activity, dimensions);
        }

        return activity is not null ? activity : NoOpDisposable.Instance;
    }

    /// <inheritdoc />
    public void RecordSuccess(
        string targetService,
        string operation,
        TimeSpan duration,
        IReadOnlyDictionary<string, string>? dimensions = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(targetService);
        ArgumentException.ThrowIfNullOrWhiteSpace(operation);

        IntegrationMetricsRecorder.RecordOutcome(
            sourceService: _sourceService,
            targetService: targetService,
            operation: operation,
            status: "success",
            errorCode: null,
            duration: duration,
            extraDimensions: dimensions);
    }

    /// <inheritdoc />
    public void RecordFailure(
        string targetService,
        string operation,
        string errorCode,
        TimeSpan duration,
        IReadOnlyDictionary<string, string>? dimensions = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(targetService);
        ArgumentException.ThrowIfNullOrWhiteSpace(operation);
        ArgumentException.ThrowIfNullOrWhiteSpace(errorCode);

        IntegrationMetricsRecorder.RecordOutcome(
            sourceService: _sourceService,
            targetService: targetService,
            operation: operation,
            status: "failure",
            errorCode: errorCode,
            duration: duration,
            extraDimensions: dimensions);
    }

    private static void ApplyActivityTags(Activity activity, IReadOnlyDictionary<string, string>? dimensions)
    {
        if (dimensions is null)
        {
            return;
        }

        foreach (var (key, value) in dimensions)
        {
            if (string.IsNullOrWhiteSpace(key)
                || string.IsNullOrWhiteSpace(value)
                || IntegrationMetricDimensions.IsReserved(key))
            {
                continue;
            }

            activity.SetTag(key, value);
        }
    }

    private sealed class NoOpDisposable : IDisposable
    {
        public static readonly NoOpDisposable Instance = new();

        public void Dispose()
        {
        }
    }
}

/// <summary>
/// Shared recording logic for outbound integration metrics.
/// </summary>
internal static class IntegrationMetricsRecorder
{
    public static void RecordOutcome(
        string? sourceService,
        string targetService,
        string operation,
        string status,
        string? errorCode,
        TimeSpan duration,
        IReadOnlyDictionary<string, string>? extraDimensions)
    {
        var callTags = BuildTags(
            sourceService,
            targetService,
            operation,
            status,
            errorCode,
            extraDimensions,
            includeErrorCode: true);

        OntogonyDiagnostics.IntegrationCallCount.Add(1, callTags);

        if (string.Equals(status, "failure", StringComparison.Ordinal))
        {
            OntogonyDiagnostics.IntegrationErrorCount.Add(1, callTags);
        }

        var durationTags = BuildTags(
            sourceService,
            targetService,
            operation,
            status,
            errorCode: null,
            extraDimensions,
            includeErrorCode: false);

        if (duration > TimeSpan.Zero)
        {
            OntogonyDiagnostics.IntegrationDurationMs.Record(duration.TotalMilliseconds, durationTags);
        }
    }

    /// <summary>
    /// Records a legacy integration call using historical parameter names.
    /// </summary>
    public static void RecordLegacyCall(string integrationName, string operation, int statusCode)
    {
        var status = statusCode >= 400 || statusCode == 0 ? "failure" : "success";
        var tags = BuildTags(
            sourceService: null,
            targetService: integrationName,
            operation: operation,
            status: status,
            errorCode: null,
            extraDimensions: null,
            includeErrorCode: false,
            httpStatus: statusCode);

        OntogonyDiagnostics.IntegrationCallCount.Add(1, tags);
    }

    /// <summary>
    /// Records a legacy integration error using historical parameter names.
    /// </summary>
    public static void RecordLegacyError(string integrationName, string operation, int statusCode)
    {
        var tags = BuildTags(
            sourceService: null,
            targetService: integrationName,
            operation: operation,
            status: "failure",
            errorCode: statusCode == 0 ? "transport_error" : $"http_{statusCode}",
            extraDimensions: null,
            includeErrorCode: true,
            httpStatus: statusCode);

        OntogonyDiagnostics.IntegrationErrorCount.Add(1, tags);
    }

    /// <summary>
    /// Records a legacy integration duration using historical parameter names.
    /// </summary>
    public static void RecordLegacyDuration(string integrationName, string operation, double durationMs)
    {
        if (durationMs <= 0)
        {
            return;
        }

        var tags = BuildTags(
            sourceService: null,
            targetService: integrationName,
            operation: operation,
            status: "unknown",
            errorCode: null,
            extraDimensions: null,
            includeErrorCode: false);

        OntogonyDiagnostics.IntegrationDurationMs.Record(durationMs, tags);
    }

    private static KeyValuePair<string, object?>[] BuildTags(
        string? sourceService,
        string targetService,
        string operation,
        string status,
        string? errorCode,
        IReadOnlyDictionary<string, string>? extraDimensions,
        bool includeErrorCode,
        int? httpStatus = null)
    {
        var tags = new List<KeyValuePair<string, object?>>(capacity: 8)
        {
            new(IntegrationMetricDimensions.TargetService, targetService),
            new(IntegrationMetricDimensions.Operation, operation),
            new(IntegrationMetricDimensions.Status, status),
        };

        if (!string.IsNullOrWhiteSpace(sourceService))
        {
            tags.Add(new(IntegrationMetricDimensions.SourceService, sourceService));
        }

        if (includeErrorCode && !string.IsNullOrWhiteSpace(errorCode))
        {
            tags.Add(new(IntegrationMetricDimensions.ErrorCode, errorCode));
        }

        if (httpStatus is not null)
        {
            tags.Add(new(IntegrationMetricDimensions.HttpStatus, httpStatus.Value));
        }

        if (extraDimensions is not null)
        {
            foreach (var (key, value) in extraDimensions)
            {
                if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(value))
                {
                    continue;
                }

                if (IntegrationMetricDimensions.IsReserved(key))
                {
                    continue;
                }

                tags.Add(new KeyValuePair<string, object?>(key, value));
            }

            if (httpStatus is null
                && extraDimensions.TryGetValue(IntegrationMetricDimensions.HttpStatus, out var httpStatusValue)
                && int.TryParse(httpStatusValue, out var parsedHttpStatus))
            {
                tags.Add(new(IntegrationMetricDimensions.HttpStatus, parsedHttpStatus));
            }
        }

        return tags.ToArray();
    }
}
