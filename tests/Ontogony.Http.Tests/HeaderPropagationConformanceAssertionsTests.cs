using Ontogony.Http;
using Ontogony.Testing;
using Xunit;

namespace Ontogony.Http.Tests;

public sealed class HeaderPropagationConformanceAssertionsTests
{
    [Fact]
    public async Task AssertIntegrationHandlerPropagatesScenarioAsync_emits_frozen_headers()
    {
        await HeaderPropagationConformanceAssertions.AssertIntegrationHandlerPropagatesScenarioAsync(
            new PropagationHeaderScenario(
                TraceParent: "00-4bf92f3577b34da6a3ce929d0e0e4736-00f067aa0ba902b7-01",
                CorrelationId: "corr-op-42",
                ActorId: "actor-1",
                ActorType: "service",
                ActorRoles: "operator,admin",
                IdempotencyKey: "idem-42",
                AllagmaRunId: "run-abc",
                TraceId: "trace-abc"));
    }
}
