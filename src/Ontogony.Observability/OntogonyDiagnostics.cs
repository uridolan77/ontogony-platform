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
}
