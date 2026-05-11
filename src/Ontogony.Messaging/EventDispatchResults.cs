namespace Ontogony.Messaging;

/// <summary>
/// Outcome of invoking a single in-process event handler.
/// </summary>
public sealed record EventHandlerDispatchResult(
    int HandlerIndex,
    string HandlerDescription,
    bool Succeeded,
    double DurationMs,
    string? ErrorMessage);

/// <summary>
/// Describes a handler that threw during dispatch.
/// </summary>
public sealed record EventDispatchFailure(int HandlerIndex, string HandlerDescription, Exception Exception);

/// <summary>
/// Summary of a publish attempt including per-handler dispatch diagnostics.
/// </summary>
public sealed record EventPublishResult(
    EventPublisherOperationMode Mode,
    bool CapturedInSink,
    IReadOnlyList<EventHandlerDispatchResult> HandlerResults,
    IReadOnlyList<EventDispatchFailure> Failures);
