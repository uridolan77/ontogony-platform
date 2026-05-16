using Ontogony.Contracts.Events;
using Ontogony.Observability;
using Xunit;

namespace Ontogony.Observability.Tests;

public sealed class CorrelationContextTests
{
    [Fact]
    public void FromHeaders_Custom_Accepted_Header_Resolves_When_Canonical_Missing()
    {
        var headers = new Dictionary<string, string?>
        {
            ["X-Custom-Trace"] = "trace-custom"
        };

        var state = OntogonyCorrelationContext.FromHeaders(
            headers,
            OntogonyEventHeaders.TraceId,
            ["X-Custom-Trace"]);

        Assert.NotNull(state);
        Assert.Equal("trace-custom", state!.TraceId);
    }

    [Fact]
    public void FromHeaders_Canonical_Header_Wins_Over_Custom_Accepted_Header()
    {
        var headers = new Dictionary<string, string?>
        {
            [OntogonyEventHeaders.TraceId] = "trace-canonical",
            ["X-Custom-Trace"] = "trace-custom"
        };

        var state = OntogonyCorrelationContext.FromHeaders(
            headers,
            OntogonyEventHeaders.TraceId,
            ["X-Custom-Trace"]);

        Assert.NotNull(state);
        Assert.Equal("trace-canonical", state!.TraceId);
    }

    [Fact]
    public void Push_Restores_Previous_Context_On_Dispose()
    {
        using var outer = OntogonyCorrelationContext.Push("outer");
        Assert.Equal("outer", OntogonyCorrelationContext.TraceId);

        using (OntogonyCorrelationContext.Push("inner"))
        {
            Assert.Equal("inner", OntogonyCorrelationContext.TraceId);
        }

        Assert.Equal("outer", OntogonyCorrelationContext.TraceId);
    }

    [Fact]
    public void CurrentOrCreate_Returns_Same_State_When_Already_Set()
    {
        using var _ = OntogonyCorrelationContext.Push(new CorrelationState("trace-existing", "op-existing"));

        var state = OntogonyCorrelationContext.CurrentOrCreate();

        Assert.Equal("trace-existing", state.TraceId);
        Assert.Equal("op-existing", state.OperationId);
    }

    [Fact]
    public void ToMetadata_Exports_Known_Headers()
    {
        using var _ = OntogonyCorrelationContext.Push(new CorrelationState(
            "trace-1",
            "op-1",
            TenantId: "tenant-1",
            WorkspaceId: "workspace-1",
            ProjectId: "project-1",
            ActorId: "actor-1",
            SessionId: "session-1",
            TraceParent: "00-4bf92f3577b34da6a3ce929d0e0e4736-1111111111111111-01",
            TraceState: "vendor=1"));

        var metadata = OntogonyCorrelationContext.ToMetadata();

        Assert.Equal("trace-1", metadata[OntogonyEventHeaders.TraceId]);
        Assert.Equal("actor-1", metadata[OntogonyEventHeaders.ActorId]);
        Assert.Equal("tenant-1", metadata[OntogonyEventHeaders.TenantId]);
        Assert.Equal("workspace-1", metadata[OntogonyEventHeaders.WorkspaceId]);
        Assert.Equal("project-1", metadata[OntogonyEventHeaders.ProjectId]);
        Assert.Equal("session-1", metadata[OntogonyEventHeaders.SessionId]);
        Assert.Equal("00-4bf92f3577b34da6a3ce929d0e0e4736-1111111111111111-01", metadata[OntogonyEventHeaders.TraceParent]);
        Assert.Equal("vendor=1", metadata[OntogonyEventHeaders.TraceState]);
    }

    [Fact]
    public void FromHeaders_Uses_Incoming_Correlation_Header_As_OperationId()
    {
        var headers = new Dictionary<string, string?>
        {
            [OntogonyEventHeaders.TraceId] = "trace-1",
            [OntogonyEventHeaders.CorrelationId] = "corr-inbound-1"
        };

        var state = OntogonyCorrelationContext.FromHeaders(headers);

        Assert.NotNull(state);
        Assert.Equal("trace-1", state!.TraceId);
        Assert.Equal("corr-inbound-1", state.OperationId);
    }

    [Fact]
    public void FromHeaders_Accepts_Legacy_Correlation_Header_As_OperationId()
    {
        var headers = new Dictionary<string, string?>
        {
            [OntogonyEventHeaders.TraceId] = "trace-1",
            [OntogonyEventHeaders.LegacyCorrelationId] = "corr-legacy-1"
        };

        var state = OntogonyCorrelationContext.FromHeaders(headers);

        Assert.NotNull(state);
        Assert.Equal("corr-legacy-1", state!.OperationId);
    }

    [Fact]
    public void FromHeaders_Uses_Ontogony_Header_Over_Legacy_Header()
    {
        var headers = new Dictionary<string, string?>
        {
            [OntogonyEventHeaders.TraceId] = "trace-ontogony",
            [OntogonyEventHeaders.LegacyAthanorTraceId] = "trace-legacy"
        };

        var state = OntogonyCorrelationContext.FromHeaders(headers);

        Assert.NotNull(state);
        Assert.Equal("trace-ontogony", state!.TraceId);
    }

    [Fact]
    public void FromHeaders_Accepts_TraceParent_When_Trace_Header_Is_Missing()
    {
        var headers = new Dictionary<string, string?>
        {
            [OntogonyEventHeaders.TraceParent] = "00-4bf92f3577b34da6a3ce929d0e0e4736-1111111111111111-01"
        };

        var state = OntogonyCorrelationContext.FromHeaders(headers);

        Assert.NotNull(state);
        Assert.Equal("4bf92f3577b34da6a3ce929d0e0e4736", state!.TraceId);
    }

    [Fact]
    public void FromEnvelope_Maps_Envelope_Correlation()
    {
        var envelope = new OntogonyEnvelope<object>
        {
            EventId = "evt-1",
            EventType = "agent.run.started",
            Source = "https://tests.local/from-envelope",
            OccurredAt = DateTimeOffset.UtcNow,
            TraceId = "trace-1",
            TenantId = "tenant-1",
            WorkspaceId = "workspace-1",
            ProjectId = "project-1",
            ActorId = "actor-1",
            SessionId = "session-1",
            Protocol = ProtocolNames.Agentor,
            Payload = new { Ok = true }
        };

        var state = OntogonyCorrelationContext.FromEnvelope(envelope);

        Assert.NotNull(state);
        Assert.Equal("trace-1", state!.TraceId);
        Assert.Equal("tenant-1", state.TenantId);
        Assert.Equal("workspace-1", state.WorkspaceId);
        Assert.Equal("project-1", state.ProjectId);
        Assert.Equal("actor-1", state.ActorId);
        Assert.Equal("session-1", state.SessionId);
    }

    [Fact]
    public async Task Push_Restores_Previous_Context_After_Async_Nesting()
    {
        using var outer = OntogonyCorrelationContext.Push("outer");

        await Task.Run(async () =>
        {
            using var inner = OntogonyCorrelationContext.Push("inner");
            await Task.Delay(1);
            Assert.Equal("inner", OntogonyCorrelationContext.TraceId);
        });

        Assert.Equal("outer", OntogonyCorrelationContext.TraceId);
    }
}
