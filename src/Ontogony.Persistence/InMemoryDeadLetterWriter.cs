namespace Ontogony.Persistence;

/// <summary>
/// In-memory capture of dead-letter messages for tests and single-process diagnostics.
/// </summary>
public sealed class InMemoryDeadLetterWriter : IDeadLetterWriter
{
    private readonly List<DeadLetterMessage> _messages = [];
    private readonly object _sync = new();

    /// <inheritdoc />
    public Task WriteAsync(DeadLetterMessage deadLetter, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(deadLetter);
        lock (_sync)
        {
            _messages.Add(deadLetter);
        }

        return Task.CompletedTask;
    }

    /// <summary>Number of captured dead-letter messages.</summary>
    public int Count
    {
        get
        {
            lock (_sync)
            {
                return _messages.Count;
            }
        }
    }

    /// <summary>Returns all captured dead-letter messages.</summary>
    public IReadOnlyList<DeadLetterMessage> ReadAll()
    {
        lock (_sync)
        {
            return _messages.ToArray();
        }
    }

    /// <summary>Removes all captured dead-letter messages.</summary>
    public void Clear()
    {
        lock (_sync)
        {
            _messages.Clear();
        }
    }
}
