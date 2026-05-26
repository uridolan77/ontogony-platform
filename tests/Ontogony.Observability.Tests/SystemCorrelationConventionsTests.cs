using System.Diagnostics;
using Ontogony.Contracts.Events;
using Ontogony.Observability;
using Xunit;

namespace Ontogony.Observability.Tests;

public sealed class SystemCorrelationConventionsTests
{
    [Fact]
    public void ApplyToActivity_sets_frozen_correlation_tags()
    {
        using var listener = new ActivityListener
        {
            ShouldListenTo = _ => true,
            Sample = (ref ActivityCreationOptions<ActivityContext> _) => ActivitySamplingResult.AllData,
        };
        ActivitySource.AddActivityListener(listener);

        var source = new ActivitySource("test");
        using var activity = source.StartActivity("test.op");
        var state = new CorrelationState(
            TraceId: "trace-abc",
            OperationId: "op-123",
            ActorId: "actor-1",
            ProjectId: "proj-9");

        SystemCorrelationConventions.ApplyToActivity(activity, state);

        Assert.Equal("trace-abc", activity!.GetTagItem(OntogonySpanAttributes.TraceId));
        Assert.Equal("op-123", activity.GetTagItem(OntogonySpanAttributes.OperationId));
        Assert.Equal("actor-1", activity.GetTagItem(OntogonySpanAttributes.ActorId));
        Assert.Equal("proj-9", activity.GetTagItem(OntogonySpanAttributes.ProjectId));
    }

    [Fact]
    public void ToLogScope_aligns_with_trace_and_correlation_headers()
    {
        var state = new CorrelationState("t1", "o1");
        var scope = SystemCorrelationConventions.ToLogScope(state, "Allagma.Api");

        Assert.Equal("t1", scope[SystemCorrelationConventions.LogScopeTraceId]);
        Assert.Equal("o1", scope[SystemCorrelationConventions.LogScopeOperationId]);
        Assert.Equal("Allagma.Api", scope["service"]);
    }

    [Fact]
    public void CorrelationHeaderSpanMappings_include_canonical_trace_headers()
    {
        Assert.Contains(
            SystemCorrelationConventions.CorrelationHeaderSpanMappings,
            m => m.HttpHeader == OntogonyEventHeaders.TraceId
                 && m.SpanAttribute == OntogonySpanAttributes.TraceId);
        Assert.Contains(
            SystemCorrelationConventions.CorrelationHeaderSpanMappings,
            m => m.HttpHeader == OntogonyEventHeaders.CorrelationId
                 && m.SpanAttribute == OntogonySpanAttributes.OperationId);
    }
}
