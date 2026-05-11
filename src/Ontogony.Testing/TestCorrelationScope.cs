using Ontogony.Observability;

namespace Ontogony.Testing;

public sealed class TestCorrelationScope : IDisposable
{
    private readonly IDisposable _inner;

    public TestCorrelationScope(
        string traceId,
        string? operationId = null,
        string? tenantId = null,
        string? workspaceId = null,
        string? projectId = null,
        string? actorId = null,
        string? sessionId = null)
    {
        var state = new CorrelationState(
            traceId,
            operationId ?? Guid.NewGuid().ToString("n"),
            tenantId,
            workspaceId,
            projectId,
            actorId,
            sessionId);
        _inner = OntogonyCorrelationContext.Push(state);
    }

    public void Dispose() => _inner.Dispose();
}