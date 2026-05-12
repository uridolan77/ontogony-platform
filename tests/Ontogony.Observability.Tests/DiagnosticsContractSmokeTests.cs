using Ontogony.Observability;
using Xunit;

namespace Ontogony.Observability.Tests;

public sealed class DiagnosticsContractSmokeTests
{
    [Fact]
    public void ActivitySource_And_Meter_Names_Are_Stable()
    {
        Assert.Equal("Ontogony.Platform", OntogonyDiagnostics.DefaultActivitySourceName);
        Assert.Equal("Ontogony.Platform", OntogonyDiagnostics.ActivitySource.Name);
        Assert.Equal("Ontogony.Platform", OntogonyDiagnostics.Meter.Name);
    }

    [Fact]
    public void Metric_Instrument_Names_Are_Stable()
    {
        Assert.Equal("ontogony.http.server.request.count", OntogonyDiagnostics.HttpServerRequestCount.Name);
        Assert.Equal("ontogony.http.server.error.count", OntogonyDiagnostics.HttpServerErrorCount.Name);
        Assert.Equal("ontogony.http.server.duration.ms", OntogonyDiagnostics.HttpServerDurationMs.Name);

        Assert.Equal("ontogony.integration.call.count", OntogonyDiagnostics.IntegrationCallCount.Name);
        Assert.Equal("ontogony.integration.error.count", OntogonyDiagnostics.IntegrationErrorCount.Name);
        Assert.Equal("ontogony.integration.duration.ms", OntogonyDiagnostics.IntegrationDurationMs.Name);

        Assert.Equal("ontogony.event.publish.count", OntogonyDiagnostics.EventPublishCount.Name);
        Assert.Equal("ontogony.event.dispatch.count", OntogonyDiagnostics.EventDispatchCount.Name);
        Assert.Equal("ontogony.event.dispatch.failure.count", OntogonyDiagnostics.EventDispatchFailureCount.Name);
        Assert.Equal("ontogony.event.handler.duration.ms", OntogonyDiagnostics.EventHandlerDurationMs.Name);
    }

    [Fact]
    public void Span_Attribute_Keys_Are_Stable()
    {
        Assert.Equal("ontogony.trace_id", OntogonySpanAttributes.TraceId);
        Assert.Equal("ontogony.operation_id", OntogonySpanAttributes.OperationId);
        Assert.Equal("ontogony.actor_id", OntogonySpanAttributes.ActorId);
        Assert.Equal("ontogony.tenant_id", OntogonySpanAttributes.TenantId);
        Assert.Equal("ontogony.workspace_id", OntogonySpanAttributes.WorkspaceId);
        Assert.Equal("ontogony.project_id", OntogonySpanAttributes.ProjectId);
        Assert.Equal("ontogony.session_id", OntogonySpanAttributes.SessionId);
        Assert.Equal("ontogony.protocol", OntogonySpanAttributes.Protocol);
        Assert.Equal("ontogony.event_type", OntogonySpanAttributes.EventType);
    }
}
