using System.Diagnostics;
using Ontogony.AI.Contracts;
using Ontogony.Artifacts;
using Ontogony.Errors;
using Ontogony.Execution;
using Ontogony.Hosting;
using Ontogony.Http;
using Ontogony.Idempotency;
using Ontogony.Logging;
using Ontogony.Observability;
using Ontogony.Persistence;
using Ontogony.Persistence.Postgres;
using Ontogony.Primitives;
using Ontogony.Redaction;
using Ontogony.Replay.Contracts;
using Ontogony.Secrets;
using Ontogony.Security;

// Compile-time smoke for the Allagma.NET v1 required package set (see docs/consumer-blueprints/allagma-dotnet-platform-readiness.md).
_ = typeof(LlmRequestEnvelope);
_ = typeof(ArtifactRef);
_ = typeof(IExecutionJournal);
_ = typeof(ReplayManifest);
_ = typeof(IOutboxWriter);
_ = typeof(PostgresOutboxStore);
_ = typeof(Ontogony.Primitives.SystemClock);
_ = typeof(InMemoryIdempotencyLedger);
_ = typeof(RequestTracingMiddleware);
_ = typeof(OntogonyExceptionHandlingMiddleware);
_ = typeof(ServiceIdentityOptions);
_ = typeof(OntogonyLoggingScopeMiddleware);
_ = typeof(IRedactor);
_ = typeof(SecretRef);
_ = typeof(IIntegrationOperationMeter);
_ = typeof(IntegrationMetricDimensions);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOntogonyServiceDefaults(builder.Configuration, o =>
{
    o.ServiceName = "allagma-dotnet-skeleton";
    o.ServiceVersion = "0.0.0";
    o.UseServiceIdentityBodyHashPreload = false;
});

builder.Services.AddOntogonyLogging(o =>
{
    o.ServiceName = "allagma-dotnet-skeleton";
    o.ServiceVersion = "0.0.0";
});

builder.Services.AddOntogonyRedaction();
builder.Services.AddOntogonySecrets();
builder.Services.AddOntogonyHeaderActorContext();

builder.Services.AddOntogonyIntegrationHttpClient("partner-alpha", _ => new HttpIntegrationOptions
{
    BaseUrl = "https://example.invalid/",
    TimeoutSeconds = 30
});

builder.Services.AddOntogonyIntegrationHttpClient("partner-beta", _ => new HttpIntegrationOptions
{
    BaseUrl = "https://example.invalid/",
    TimeoutSeconds = 30
});

builder.Services.AddOntogonyInMemoryArtifactStore();
builder.Services.AddOntogonyInMemoryExecutionJournal();
builder.Services.AddOntogonyInMemoryOutboxStore();
builder.Services.AddSingleton<IIdempotencyLedger, InMemoryIdempotencyLedger>();

var app = builder.Build();

_ = app.Services.GetRequiredService<IRedactor>();
_ = app.Services.GetRequiredService<ISecretFingerprintService>();
_ = app.Services.GetRequiredService<IIntegrationOperationMeter>();
_ = app.Services.GetRequiredService<ICurrentActorAccessor>();
_ = new SecretRef("demo", Fingerprint: app.Services.GetRequiredService<ISecretFingerprintService>().ComputeFingerprint("unit"));

app.UseOntogonyRequestTracing();
app.UseOntogonyLoggingScope();
app.UseOntogonyExceptionHandling();

app.MapGet("/", () => Results.Ok(new { ok = true }));

app.MapGet("/integration-metrics-smoke", (IIntegrationOperationMeter meter) =>
{
    var started = Stopwatch.GetTimestamp();
    using (meter.StartCall("partner-alpha", "SmokeOperation"))
    {
        meter.RecordSuccess("partner-alpha", "SmokeOperation", Stopwatch.GetElapsedTime(started));
    }

    return Results.Ok();
});

await app.RunAsync();
