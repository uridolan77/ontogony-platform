using Ontogony.Contracts.Events;

namespace Ontogony.Messaging;

public sealed class InMemoryEventSink
{
    private readonly List<object> _events = [];
    private readonly object _sync = new();

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

    public void Append<TPayload>(OntogonyEnvelope<TPayload> envelope)
    {
        ArgumentNullException.ThrowIfNull(envelope);

        lock (_sync)
        {
            _events.Add(envelope);
        }
    }

    public IReadOnlyList<object> ReadAll()
    {
        lock (_sync)
        {
            return _events.ToArray();
        }
    }

    public IReadOnlyList<OntogonyEnvelope<TPayload>> ReadAll<TPayload>()
    {
        lock (_sync)
        {
            return _events.OfType<OntogonyEnvelope<TPayload>>().ToArray();
        }
    }

    public void Clear()
    {
        lock (_sync)
        {
            _events.Clear();
        }
    }
}