namespace Ontogony.Testing;

/// <summary>
/// Expected propagation values for <see cref="HeaderPropagationConformanceAssertions"/> outbound proofs.
/// </summary>
/// <param name="TraceParent">W3C <c>traceparent</c> value.</param>
/// <param name="CorrelationId">Operation correlation (canonical or legacy alias).</param>
/// <param name="ActorId">Actor identifier.</param>
/// <param name="ActorType">Actor type classifier.</param>
/// <param name="ActorRoles">Comma-separated roles.</param>
/// <param name="IdempotencyKey">Canonical <c>X-Ontogony-Idempotency-Key</c>.</param>
/// <param name="AllagmaRunId">Optional <c>X-Allagma-Run-Id</c> (AdditionalHeaders).</param>
/// <param name="TraceId">Optional <c>X-Ontogony-Trace-Id</c> from correlation context.</param>
public sealed record PropagationHeaderScenario(
    string? TraceParent = null,
    string? CorrelationId = null,
    string? ActorId = null,
    string? ActorType = null,
    string? ActorRoles = null,
    string? IdempotencyKey = null,
    string? AllagmaRunId = null,
    string? TraceId = null);
