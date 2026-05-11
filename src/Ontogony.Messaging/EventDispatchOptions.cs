namespace Ontogony.Messaging;

/// <summary>
/// Options controlling validation, hashing, dispatch failure policy, and observability for <see cref="InMemoryEventPublisher"/>.
/// </summary>
public sealed class EventDispatchOptions
{
    /// <summary>
    /// When true, handler exceptions after earlier handlers have run are collected and an <see cref="AggregateException"/> is thrown at the end of dispatch.
    /// When false, the first handler exception propagates immediately and later handlers do not run.
    /// </summary>
    public bool ContinueOnHandlerException { get; set; }

    /// <summary>When true, missing <c>PayloadHash</c> is populated before capture/dispatch.</summary>
    public bool ComputePayloadHash { get; set; }

    /// <summary>When true, enforces mechanical required envelope fields before publish.</summary>
    public bool ValidateRequiredEnvelopeFields { get; set; } = true;

    /// <summary>Controls sink capture, handler invocation, and observability counters.</summary>
    public EventPublisherOperationMode OperationMode { get; set; } = EventPublisherOperationMode.PublishAndDispatch;

    /// <summary>
    /// When true (default), records OpenTelemetry counters for publish and dispatch paths (except <see cref="EventPublisherOperationMode.CaptureOnly"/>).
    /// </summary>
    public bool RecordObservabilityMetrics { get; set; } = true;
}