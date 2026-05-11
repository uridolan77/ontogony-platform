using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace Ontogony.Observability;

public static class OntogonyDiagnostics
{
    public const string DefaultActivitySourceName = "Ontogony.Platform";
    public static readonly ActivitySource ActivitySource = new(DefaultActivitySourceName, "0.1.0");
    public static readonly Meter Meter = new(DefaultActivitySourceName, "0.1.0");

    public static readonly Counter<long> HttpServerRequestCount =
        Meter.CreateCounter<long>("ontogony.http.server.request.count", description: "HTTP requests observed by Ontogony middleware.");

    public static readonly Counter<long> HttpServerErrorCount =
        Meter.CreateCounter<long>("ontogony.http.server.error.count", description: "HTTP errors observed by Ontogony middleware.");

    public static readonly Histogram<double> HttpServerDurationMs =
        Meter.CreateHistogram<double>("ontogony.http.server.duration.ms", unit: "ms", description: "HTTP request duration in milliseconds.");

    public static readonly Counter<long> IntegrationCallCount =
        Meter.CreateCounter<long>("ontogony.integration.call.count", description: "Outbound integration call count.");

    public static readonly Counter<long> IntegrationErrorCount =
        Meter.CreateCounter<long>("ontogony.integration.error.count", description: "Outbound integration call error count.");

    public static readonly Histogram<double> IntegrationDurationMs =
        Meter.CreateHistogram<double>("ontogony.integration.duration.ms", unit: "ms", description: "Outbound integration call duration in milliseconds.");

    public static readonly Counter<long> EventPublishedCount =
        Meter.CreateCounter<long>("ontogony.event.published.count", description: "Count of events published through Ontogony abstractions.");

    public static readonly Counter<long> EventHandledCount =
        Meter.CreateCounter<long>("ontogony.event.handled.count", description: "Count of events handled through Ontogony abstractions.");
}

public static class OntogonyMetrics
{
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

    public static void RecordHttpDuration(string service, string method, double durationMs)
    {
        OntogonyDiagnostics.HttpServerDurationMs.Record(durationMs,
            new("service", service),
            new("method", method));
    }

    public static void RecordHttpError(string service, string method, int statusCode)
    {
        OntogonyDiagnostics.HttpServerErrorCount.Add(1,
            new("service", service),
            new("method", method),
            new("status_code", statusCode));
    }

    public static void RecordIntegrationCall(string integrationName, string operation, int statusCode)
    {
        OntogonyDiagnostics.IntegrationCallCount.Add(1,
            new("integration", integrationName),
            new("operation", operation),
            new("status_code", statusCode));
    }

    public static void RecordIntegrationDuration(string integrationName, string operation, double durationMs)
    {
        OntogonyDiagnostics.IntegrationDurationMs.Record(durationMs,
            new("integration", integrationName),
            new("operation", operation));
    }

    public static void RecordIntegrationError(string integrationName, string operation, int statusCode)
    {
        OntogonyDiagnostics.IntegrationErrorCount.Add(1,
            new("integration", integrationName),
            new("operation", operation),
            new("status_code", statusCode));
    }

    public static void RecordEventPublished(string eventType, string protocol)
    {
        OntogonyDiagnostics.EventPublishedCount.Add(1,
            new("event_type", eventType),
            new("protocol", protocol));
    }

    public static void RecordEventHandled(string eventType, string protocol)
    {
        OntogonyDiagnostics.EventHandledCount.Add(1,
            new("event_type", eventType),
            new("protocol", protocol));
    }
}
