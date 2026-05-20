using Ontogony.Contracts.Events;
using Xunit;

namespace Ontogony.Infrastructure.Tests;

public sealed class HeaderConstantsSnapshotTests
{
    /// <summary>
    /// Snapshot test for OntogonyEventHeaders constants.
    /// If this test fails, you are changing a public header constant which may break consuming services.
    /// Update the expected values below only after confirming compatibility and documenting in CHANGELOG and migrations/.
    /// </summary>
    [Fact]
    public void OntogonyEventHeaders_Constants_AreStable()
    {
        // Arrange & Assert
        Assert.Equal("traceparent", OntogonyEventHeaders.TraceParent);
        Assert.Equal("tracestate", OntogonyEventHeaders.TraceState);
        Assert.Equal("X-Ontogony-Trace-Id", OntogonyEventHeaders.TraceId);
        Assert.Equal("X-Athanor-Trace-Id", OntogonyEventHeaders.LegacyAthanorTraceId);
        Assert.Equal("X-Agentor-Trace-Id", OntogonyEventHeaders.LegacyAgentorTraceId);
        Assert.Equal("X-Conexus-Request-Id", OntogonyEventHeaders.ConexusRequestId);
        Assert.Equal("X-Ontogony-Actor-Id", OntogonyEventHeaders.ActorId);
        Assert.Equal("X-Ontogony-Tenant-Id", OntogonyEventHeaders.TenantId);
        Assert.Equal("X-Ontogony-Project-Id", OntogonyEventHeaders.ProjectId);
        Assert.Equal("X-Ontogony-Workspace-Id", OntogonyEventHeaders.WorkspaceId);
        Assert.Equal("X-Ontogony-Session-Id", OntogonyEventHeaders.SessionId);
        Assert.Equal("Idempotency-Key", OntogonyEventHeaders.IdempotencyKey);
        Assert.Equal("X-Ontogony-Protocol-Id", OntogonyEventHeaders.ProtocolId);
        Assert.Equal("X-Ontogony-Authority-Mode", OntogonyEventHeaders.AuthorityMode);
        Assert.Equal("X-Ontogony-Side-Effect-Level", OntogonyEventHeaders.SideEffectLevel);
        Assert.Equal("X-Ontogony-Correlation-Id", OntogonyEventHeaders.CorrelationId);
        Assert.Equal("X-Correlation-ID", OntogonyEventHeaders.LegacyCorrelationId);
    }

    /// <summary>
    /// Snapshot test for ProtocolNames constants.
    /// If this test fails, you are changing a public protocol constant which may break consuming services.
    /// Update the expected values below only after confirming compatibility and documenting in CHANGELOG and migrations/.
    /// </summary>
    [Fact]
    public void ProtocolNames_Constants_AreStable()
    {
        // Arrange & Assert
        Assert.Equal("ag-ui", ProtocolNames.AgUi);
        Assert.Equal("mcp", ProtocolNames.Mcp);
        Assert.Equal("a2a", ProtocolNames.A2A);
        Assert.Equal("otel", ProtocolNames.OpenTelemetry);
        Assert.Equal("cloudevents", ProtocolNames.CloudEvents);
        Assert.Equal("conexus", ProtocolNames.Conexus);
        Assert.Equal("agentor", ProtocolNames.Agentor);
        Assert.Equal("athanor", ProtocolNames.Athanor);
        Assert.Equal("vercel-ai-sdk", ProtocolNames.VercelAiSdk);
        Assert.Equal("openai-agents", ProtocolNames.OpenAiAgents);
        Assert.Equal("langgraph", ProtocolNames.LangGraph);
    }
}
