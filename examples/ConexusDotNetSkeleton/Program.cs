using Ontogony.AI.Contracts;
using Ontogony.Artifacts;
using Ontogony.Contracts.Events;
using Ontogony.Errors;
using Ontogony.Execution;
using Ontogony.Hashing;
using Ontogony.Hosting;
using Ontogony.Http;
using Ontogony.Idempotency;
using Ontogony.Observability;
using Ontogony.Security;

// Compile-time smoke for the Conexus.NET v1 required package set (see docs/consumer-blueprints/conexus-dotnet-platform-readiness.md).
_ = typeof(LlmRequestEnvelope);
_ = typeof(ArtifactRef);
_ = typeof(OntogonyEnvelope<LlmRequestEnvelope>);
_ = typeof(IExecutionJournal);
_ = typeof(CanonicalJson);
_ = new InMemoryIdempotencyLedger();
_ = typeof(RequestTracingMiddleware);
_ = typeof(OntogonyExceptionHandlingMiddleware);
_ = typeof(ServiceIdentityOptions);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOntogonyServiceDefaults(builder.Configuration, o =>
{
    o.ServiceName = "conexus-dotnet-skeleton";
    o.ServiceVersion = "0.0.0";
    o.UseServiceIdentityBodyHashPreload = false;
});

builder.Services.AddOntogonyIntegrationHttpClient("sk", _ => new HttpIntegrationOptions
{
    BaseUrl = "https://example.invalid/",
    TimeoutSeconds = 30
});

builder.Services.AddOntogonyInMemoryArtifactStore();
builder.Services.AddOntogonyInMemoryExecutionJournal();

var app = builder.Build();

app.UseOntogonyServiceDefaults();
app.MapGet("/", () => Results.Ok(new { ok = true }));

await app.RunAsync();
