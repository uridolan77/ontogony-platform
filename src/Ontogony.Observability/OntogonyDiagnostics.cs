using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace Ontogony.Observability;

/// <summary>
/// Shared <see cref="ActivitySource"/> and <see cref="Meter"/> for Ontogony platform instrumentation.
/// </summary>
public static class OntogonyDiagnostics
{
    /// <summary>Default activity source logical name.</summary>
    public const string DefaultActivitySourceName = "Ontogony.Platform";

    /// <summary>Activity source for HTTP and integration spans.</summary>
    public static readonly ActivitySource ActivitySource = new(DefaultActivitySourceName, "0.1.0");

    /// <summary>OpenTelemetry meter for counters and histograms.</summary>
    public static readonly Meter Meter = new(DefaultActivitySourceName, "0.1.0");

    /// <summary>HTTP server request counter.</summary>
    public static readonly Counter<long> HttpServerRequestCount =
        Meter.CreateCounter<long>("ontogony.http.server.request.count", description: "HTTP requests observed by Ontogony middleware.");

    /// <summary>HTTP server error counter.</summary>
    public static readonly Counter<long> HttpServerErrorCount =
        Meter.CreateCounter<long>("ontogony.http.server.error.count", description: "HTTP errors observed by Ontogony middleware.");

    /// <summary>HTTP server duration histogram (milliseconds).</summary>
    public static readonly Histogram<double> HttpServerDurationMs =
        Meter.CreateHistogram<double>("ontogony.http.server.duration.ms", unit: "ms", description: "HTTP request duration in milliseconds.");

    /// <summary>Outbound integration call counter.</summary>
    public static readonly Counter<long> IntegrationCallCount =
        Meter.CreateCounter<long>("ontogony.integration.call.count", description: "Outbound integration call count.");

    /// <summary>Outbound integration error counter.</summary>
    public static readonly Counter<long> IntegrationErrorCount =
        Meter.CreateCounter<long>("ontogony.integration.error.count", description: "Outbound integration call error count.");

    /// <summary>Outbound integration duration histogram (milliseconds).</summary>
    public static readonly Histogram<double> IntegrationDurationMs =
        Meter.CreateHistogram<double>("ontogony.integration.duration.ms", unit: "ms", description: "Outbound integration call duration in milliseconds.");

    /// <summary>Event publish accepted counter.</summary>
    public static readonly Counter<long> EventPublishCount =
        Meter.CreateCounter<long>("ontogony.event.publish.count", description: "Event publish attempts accepted into the publisher pipeline.");

    /// <summary>Successful in-process dispatch counter.</summary>
    public static readonly Counter<long> EventDispatchCount =
        Meter.CreateCounter<long>("ontogony.event.dispatch.count", description: "Successful in-process handler dispatch completions.");

    /// <summary>In-process dispatch failure counter.</summary>
    public static readonly Counter<long> EventDispatchFailureCount =
        Meter.CreateCounter<long>("ontogony.event.dispatch.failure.count", description: "In-process handler dispatch failures (exceptions).");

    /// <summary>Per-handler dispatch duration histogram (milliseconds).</summary>
    public static readonly Histogram<double> EventHandlerDurationMs =
        Meter.CreateHistogram<double>("ontogony.event.handler.duration.ms", unit: "ms", description: "Per-handler dispatch duration in milliseconds.");
}

/// <summary>
/// Helper methods that record common measurements on <see cref="OntogonyDiagnostics"/> instruments.
/// </summary>
public static class OntogonyMetrics
{
    /// <summary>Records one HTTP server request and optional duration/error side effects.</summary>
    public static void RecordHttpRequest(string service, string method, int statusCode, double durationMs, bool isError)
    {
        OntogonyDiagnostics.HttpServerRequestCount.Add(1,
            new("service", service),
            new("method", method),
            new("status_code", statusCode));

        if (isError)
        {
            OntogonyDiagnostics.HttpServerErrorCount.Add(1,
                new("service", service),
                new("method", method),
                new("status_code", statusCode));
        }

        if (durationMs > 0)
        {
            OntogonyDiagnostics.HttpServerDurationMs.Record(durationMs,
                new("service", service),
                new("method", method));
        }
    }

    /// <summary>Records HTTP duration only.</summary>
    public static void RecordHttpDuration(string service, string method, double durationMs)
    {
        OntogonyDiagnostics.HttpServerDurationMs.Record(durationMs,
            new("service", service),
            new("method", method));
    }

    /// <summary>Records an HTTP server error.</summary>
    public static void RecordHttpError(string service, string method, int statusCode)
    {
        OntogonyDiagnostics.HttpServerErrorCount.Add(1,
            new("service", service),
            new("method", method),
            new("status_code", statusCode));
    }

    /// <summary>Records one outbound integration call.</summary>
    public static void RecordIntegrationCall(string integrationName, string operation, int statusCode)
    {
        OntogonyDiagnostics.IntegrationCallCount.Add(1,
            new("integration", integrationName),
            new("operation", operation),
            new("status_code", statusCode));
    }

    /// <summary>Records outbound integration duration.</summary>
    public static void RecordIntegrationDuration(string integrationName, string operation, double durationMs)
    {
        OntogonyDiagnostics.IntegrationDurationMs.Record(durationMs,
            new("integration", integrationName),
            new("operation", operation));
    }

    /// <summary>Records an outbound integration error.</summary>
    public static void RecordIntegrationError(string integrationName, string operation, int statusCode)
    {
        OntogonyDiagnostics.IntegrationErrorCount.Add(1,
            new("integration", integrationName),
            new("operation", operation),
            new("status_code", statusCode));
    }

    /// <summary>Records an accepted event publish.</summary>
    public static void RecordEventPublish(string eventType, string protocol, string operationMode)
    {
        OntogonyDiagnostics.EventPublishCount.Add(1,
            new("event_type", eventType),
            new("protocol", protocol),
            new("operation_mode", operationMode));
    }

    /// <summary>Records a successful event dispatch.</summary>
    public static void RecordEventDispatch(string eventType, string protocol, string operationMode)
    {
        OntogonyDiagnostics.EventDispatchCount.Add(1,
            new("event_type", eventType),
            new("protocol", protocol),
            new("operation_mode", operationMode));
    }

    /// <summary>Records a failed event dispatch.</summary>
    public static void RecordEventDispatchFailure(string eventType, string protocol, string operationMode)
    {
        OntogonyDiagnostics.EventDispatchFailureCount.Add(1,
            new("event_type", eventType),
            new("protocol", protocol),
            new("operation_mode", operationMode));
    }

    /// <summary>Records handler duration for an event dispatch.</summary>
    public static void RecordEventHandlerDuration(
        string eventType,
        string protocol,
        string operationMode,
        string handlerDescription,
        double durationMs)
    {
        OntogonyDiagnostics.EventHandlerDurationMs.Record(durationMs,
            new("event_type", eventType),
            new("protocol", protocol),
            new("operation_mode", operationMode),
            new("handler_description", handlerDescription));
    }
}
