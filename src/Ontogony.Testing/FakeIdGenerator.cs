using Ontogony.Primitives;

namespace Ontogony.Testing;

public sealed class FakeIdGenerator : IIdGenerator
{
    private readonly Queue<Guid> _queuedGuids = new();
    private int _counter;

    public void Enqueue(Guid guid)
    {
        _queuedGuids.Enqueue(guid);
    }

    public Guid NewGuid()
    {
        if (_queuedGuids.Count > 0)
        {
            return _queuedGuids.Dequeue();
        }

        _counter++;
        return Guid.ParseExact($"00000000-0000-0000-0000-{_counter:000000000000}", "D");
    }

    public string NewId(string prefix)
    {
        if (string.IsNullOrWhiteSpace(prefix))
        {
            throw new ArgumentException("prefix cannot be null or whitespace.", nameof(prefix));
        }

        return $"{prefix}_{NewGuid():N}";
    }
}