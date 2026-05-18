using System.Reflection;
using Ontogony.AI.Contracts;
using Ontogony.Artifacts;
using Ontogony.Configuration;
using Ontogony.Contracts.Events;
using Ontogony.Errors;
using Ontogony.Evaluation.Contracts;
using Ontogony.Topology.Contracts;
using Ontogony.Execution;
using Ontogony.Hashing;
using Ontogony.Hosting;
using Ontogony.Http;
using Ontogony.Idempotency;
using Ontogony.Logging;
using Ontogony.Messaging;
using Ontogony.Observability;
using Ontogony.Persistence;
using Ontogony.Persistence.Postgres;
using Ontogony.Primitives;
using Ontogony.ProtocolIngress;
using Ontogony.Quotas;
using Ontogony.Redaction;
using Ontogony.Replay.Contracts;
using Ontogony.Secrets;
using Ontogony.Security;
using Ontogony.Testing;
using PublicApiGenerator;
using VerifyXunit;

namespace Ontogony.PublicApi.Tests;

public class ShippingAssemblyPublicApiTests
{
    public static TheoryData<string, Assembly> Assemblies { get; } = Build();

    private static TheoryData<string, Assembly> Build()
    {
        var data = new TheoryData<string, Assembly>();
        void Add(string name, Assembly asm) => data.Add(name, asm);

        Add("Ontogony.AI.Contracts", typeof(LlmRequestEnvelope).Assembly);
        Add("Ontogony.Artifacts", typeof(ArtifactRef).Assembly);
        Add("Ontogony.Configuration", typeof(EnvironmentGuard).Assembly);
        Add("Ontogony.Contracts", typeof(OntogonyEnvelope<>).Assembly);
        Add("Ontogony.Errors", typeof(ApiError).Assembly);
        Add("Ontogony.Evaluation.Contracts", typeof(EvaluationRunRecord).Assembly);
        Add("Ontogony.Topology.Contracts", typeof(TaskClassificationRecord).Assembly);
        Add("Ontogony.Execution", typeof(IExecutionJournal).Assembly);
        Add("Ontogony.Hashing", typeof(CanonicalJson).Assembly);
        Add("Ontogony.Hosting", typeof(OntogonyHostingExtensions).Assembly);
        Add("Ontogony.Http", typeof(HttpIntegrationOptions).Assembly);
        Add("Ontogony.Idempotency", typeof(InMemoryIdempotencyLedger).Assembly);
        Add("Ontogony.Logging", typeof(OntogonyLoggingOptions).Assembly);
        Add("Ontogony.Messaging", typeof(IEventPublisher).Assembly);
        Add("Ontogony.Observability", typeof(RequestTracingMiddleware).Assembly);
        Add("Ontogony.Persistence", typeof(IOutboxWriter).Assembly);
        Add("Ontogony.Persistence.Postgres", typeof(PostgresOutboxOptions).Assembly);
        Add("Ontogony.Primitives", typeof(Ontogony.Primitives.IClock).Assembly);
        Add("Ontogony.ProtocolIngress", typeof(ProtocolIngressContext).Assembly);
        Add("Ontogony.Quotas", typeof(IQuotaLedger).Assembly);
        Add("Ontogony.Redaction", typeof(IRedactor).Assembly);
        Add("Ontogony.Replay.Contracts", typeof(ReplayManifest).Assembly);
        Add("Ontogony.Secrets", typeof(SecretRef).Assembly);
        Add("Ontogony.Security", typeof(ServiceIdentityOptions).Assembly);
        Add("Ontogony.Testing", typeof(FakeClock).Assembly);
        return data;
    }

    [Theory]
    [MemberData(nameof(Assemblies))]
    public Task Public_api_matches_snapshot(string assemblyShortName, Assembly assembly) =>
        Verifier.Verify(assembly.GeneratePublicApi(CreateOptions()))
            .UseParameters(assemblyShortName);

    private static ApiGeneratorOptions CreateOptions() =>
        new()
        {
            IncludeAssemblyAttributes = false,
        };
}
