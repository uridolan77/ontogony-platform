using Ontogony.Contracts.Events;

namespace Ontogony.Messaging;

/// <summary>
/// Thread-safe in-memory list of envelopes that were <b>published</b> to a publisher (capture for tests and diagnostics).
/// This is <b>not</b> a delivery ledger, broker offset store, or outbox; it does not imply that downstream handlers succeeded.
/// </summary>
public sealed class InMemoryEventSink
{
    private readonly List<object> _events = [];
    private readonly object _sync = new();

    /// <summary>Number of captured envelopes.</summary>
    public int Count
    {
        get
        {
            lock (_sync)
            {
                return _events.Count;
            }
        }
    }

    /// <summary>Appends a published envelope snapshot.</summary>
    public void Append<TPayload>(OntogonyEnvelope<TPayload> envelope)
    {
        ArgumentNullException.ThrowIfNull(envelope);

        lock (_sync)
        {
            _events.Add(envelope);
        }
    }

    /// <summary>Returns all captured envelopes as a snapshot (unordered by type).</summary>
    public IReadOnlyList<object> ReadAll()
    {
        lock (_sync)
        {
            return _events.ToArray();
        }
    }

    /// <summary>Returns captured envelopes of the requested payload type.</summary>
    public IReadOnlyList<OntogonyEnvelope<TPayload>> ReadAll<TPayload>()
    {
        lock (_sync)
        {
            return _events.OfType<OntogonyEnvelope<TPayload>>().ToArray();
        }
    }

    /// <summary>Removes all captured envelopes.</summary>
    public void Clear()
    {
        lock (_sync)
        {
            _events.Clear();
        }
    }
}
