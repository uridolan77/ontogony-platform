using Ontogony.Contracts.Events;
using Ontogony.Hashing;

namespace Ontogony.Messaging;

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

    public async Task PublishAsync<TPayload>(OntogonyEnvelope<TPayload> envelope, CancellationToken cancellationToken = default)
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

        _sink.Append(toPublish);

        List<IEventHandler<TPayload>> snapshot;
        lock (_sync)
        {
            snapshot = _handlers.OfType<IEventHandler<TPayload>>().ToList();
        }

        if (snapshot.Count == 0)
        {
            return;
        }

        List<Exception>? exceptions = null;
        foreach (var handler in snapshot)
        {
            try
            {
                await handler.HandleAsync(toPublish, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex) when (_options.ContinueOnHandlerException)
            {
                exceptions ??= [];
                exceptions.Add(ex);
            }
        }

        if (exceptions is { Count: > 0 })
        {
            throw new AggregateException("One or more event handlers failed during dispatch.", exceptions);
        }
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