using System.Diagnostics;
using Ontogony.Contracts.Events;
using Ontogony.Hashing;
using Ontogony.Observability;

namespace Ontogony.Messaging;

/// <summary>
/// In-process publisher that optionally captures published envelopes and dispatches to registered handlers.
/// </summary>
public sealed class InMemoryEventPublisher : IEventPublisher
{
    private readonly InMemoryEventSink _sink;
    private readonly EventDispatchOptions _options;
    private readonly EnvelopePayloadHasher _envelopePayloadHasher;
    private readonly List<object> _handlers = [];
    private readonly object _sync = new();

    public InMemoryEventPublisher(
        InMemoryEventSink? sink = null,
        EventDispatchOptions? options = null,
        EnvelopePayloadHasher? envelopePayloadHasher = null)
    {
        _sink = sink ?? new InMemoryEventSink();
        _options = options ?? new EventDispatchOptions();
        _envelopePayloadHasher = envelopePayloadHasher ?? new EnvelopePayloadHasher(new PayloadHasher(new Sha256ContentHashService()));
    }

    public InMemoryEventSink Sink => _sink;

    public void Register<TPayload>(IEventHandler<TPayload> handler)
    {
        ArgumentNullException.ThrowIfNull(handler);
        lock (_sync)
        {
            _handlers.Add(handler);
        }
    }

    /// <summary>
    /// Publishes the envelope using <see cref="EventDispatchOptions"/> (including exception propagation rules).
    /// </summary>
    public async Task PublishAsync<TPayload>(OntogonyEnvelope<TPayload> envelope, CancellationToken cancellationToken = default)
    {
        var result = await PublishCoreAsync(envelope, cancellationToken).ConfigureAwait(false);
        PropagateHandlerFailures(result);
    }

    /// <summary>
    /// Publishes the envelope and returns structured dispatch diagnostics without throwing for handler failures.
    /// </summary>
    public Task<EventPublishResult> PublishWithResultAsync<TPayload>(
        OntogonyEnvelope<TPayload> envelope,
        CancellationToken cancellationToken = default) =>
        PublishCoreAsync(envelope, cancellationToken);

    private void PropagateHandlerFailures(EventPublishResult result)
    {
        if (result.Failures.Count == 0)
            return;

        if (_options.ContinueOnHandlerException)
            throw new AggregateException(
                "One or more event handlers failed during dispatch.",
                result.Failures.Select(static f => f.Exception));

        throw result.Failures[0].Exception;
    }

    private async Task<EventPublishResult> PublishCoreAsync<TPayload>(
        OntogonyEnvelope<TPayload> envelope,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(envelope);

        if (_options.ValidateRequiredEnvelopeFields)
        {
            EnsureRequiredFields(envelope);
        }

        var toPublish = envelope;
        if (_options.ComputePayloadHash && string.IsNullOrWhiteSpace(envelope.PayloadHash))
        {
            toPublish = envelope with { PayloadHash = _envelopePayloadHasher.ComputeEnvelopePayloadHash(envelope) };
        }

        var mode = _options.OperationMode;
        var captured = false;
        if (mode is EventPublisherOperationMode.PublishAndDispatch
            or EventPublisherOperationMode.PublishOnly
            or EventPublisherOperationMode.CaptureOnly)
        {
            _sink.Append(toPublish);
            captured = true;
        }

        var modeTag = mode.ToString();
        if (_options.RecordObservabilityMetrics && mode != EventPublisherOperationMode.CaptureOnly)
        {
            OntogonyMetrics.RecordEventPublish(toPublish.EventType, toPublish.Protocol, modeTag);
        }

        if (mode is EventPublisherOperationMode.CaptureOnly or EventPublisherOperationMode.PublishOnly)
        {
            return new EventPublishResult(mode, captured, [], []);
        }

        List<IEventHandler<TPayload>> snapshot;
        lock (_sync)
        {
            snapshot = _handlers.OfType<IEventHandler<TPayload>>().ToList();
        }

        if (snapshot.Count == 0)
        {
            return new EventPublishResult(mode, captured, [], []);
        }

        var handlerResults = new List<EventHandlerDispatchResult>(snapshot.Count);
        var failures = new List<EventDispatchFailure>();

        var index = 0;
        foreach (var handler in snapshot)
        {
            var description = handler.GetType().FullName ?? handler.GetType().Name;
            var started = Stopwatch.GetTimestamp();
            try
            {
                await handler.HandleAsync(toPublish, cancellationToken).ConfigureAwait(false);
                var elapsedMs = Stopwatch.GetElapsedTime(started).TotalMilliseconds;
                handlerResults.Add(new EventHandlerDispatchResult(index, description, true, elapsedMs, null));

                if (_options.RecordObservabilityMetrics)
                {
                    OntogonyMetrics.RecordEventDispatch(toPublish.EventType, toPublish.Protocol, modeTag);
                    OntogonyMetrics.RecordEventHandlerDuration(
                        toPublish.EventType,
                        toPublish.Protocol,
                        modeTag,
                        description,
                        elapsedMs);
                }
            }
            catch (Exception ex)
            {
                var elapsedMs = Stopwatch.GetElapsedTime(started).TotalMilliseconds;
                handlerResults.Add(new EventHandlerDispatchResult(index, description, false, elapsedMs, ex.Message));
                failures.Add(new EventDispatchFailure(index, description, ex));

                if (_options.RecordObservabilityMetrics)
                {
                    OntogonyMetrics.RecordEventDispatchFailure(toPublish.EventType, toPublish.Protocol, modeTag);
                    OntogonyMetrics.RecordEventHandlerDuration(
                        toPublish.EventType,
                        toPublish.Protocol,
                        modeTag,
                        description,
                        elapsedMs);
                }

                if (!_options.ContinueOnHandlerException)
                    break;
            }

            index++;
        }

        return new EventPublishResult(mode, captured, handlerResults, failures);
    }

    private static void EnsureRequiredFields<TPayload>(OntogonyEnvelope<TPayload> envelope)
    {
        if (string.IsNullOrWhiteSpace(envelope.EventId))
            throw new ArgumentException("EventId is required.", nameof(envelope));
        if (string.IsNullOrWhiteSpace(envelope.EventType))
            throw new ArgumentException("EventType is required.", nameof(envelope));
        if (string.IsNullOrWhiteSpace(envelope.Source))
            throw new ArgumentException("Source is required.", nameof(envelope));
        if (envelope.OccurredAt == default)
            throw new ArgumentException("OccurredAt is required.", nameof(envelope));
        if (string.IsNullOrWhiteSpace(envelope.TraceId))
            throw new ArgumentException("TraceId is required.", nameof(envelope));
        if (string.IsNullOrWhiteSpace(envelope.Protocol))
            throw new ArgumentException("Protocol is required.", nameof(envelope));
        if (envelope.Payload is null)
            throw new ArgumentException("Payload is required.", nameof(envelope));
    }
}
