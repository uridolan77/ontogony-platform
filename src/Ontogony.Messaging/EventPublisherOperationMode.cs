namespace Ontogony.Messaging;

/// <summary>
/// Controls how <see cref="InMemoryEventPublisher"/> captures envelopes and invokes handlers.
/// </summary>
public enum EventPublisherOperationMode
{
    /// <summary>Append to the capture sink (when present) and invoke registered handlers.</summary>
    PublishAndDispatch = 0,

    /// <summary>Append to the capture sink and record publish metrics; handlers are not invoked.</summary>
    PublishOnly = 1,

    /// <summary>
    /// Append to the capture sink only (typically tests). Does not record publish/dispatch observability counters.
    /// </summary>
    CaptureOnly = 2
}
