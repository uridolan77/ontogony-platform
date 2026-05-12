namespace Ontogony.Primitives;

/// <summary>
/// Generates opaque identifiers for correlation, idempotency keys, and similar mechanical uses.
/// </summary>
public interface IIdGenerator
{
    /// <summary>Returns a new random <see cref="Guid"/>.</summary>
    Guid NewGuid();

    /// <summary>Returns a prefixed opaque string id (format is implementation-defined).</summary>
    /// <param name="prefix">Non-empty caller-defined prefix segment.</param>
    string NewId(string prefix);
}

/// <summary>
/// <see cref="IIdGenerator"/> implementation using <see cref="Guid.NewGuid"/>.
/// </summary>
public sealed class GuidIdGenerator : IIdGenerator
{
    /// <inheritdoc />
    public Guid NewGuid() => Guid.NewGuid();

    /// <inheritdoc />
    public string NewId(string prefix) => $"{prefix}_{Guid.NewGuid():N}";
}