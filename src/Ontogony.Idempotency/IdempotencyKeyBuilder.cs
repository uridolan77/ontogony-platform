using Ontogony.Hashing;

namespace Ontogony.Idempotency;

/// <summary>
/// Constructs deterministic idempotency keys for operations with payload fingerprinting.
/// Format: {namespace}:{operation}:{version}:{sha256_hash}
/// Example: ontogony:agentor.run.start:v1:abc123def456...
/// </summary>
public sealed class IdempotencyKeyBuilder
{
    private readonly PayloadHasher _payloadHasher;
    private readonly IdempotencyKeyOptions _options;

    public IdempotencyKeyBuilder(PayloadHasher payloadHasher, IdempotencyKeyOptions? options = null)
    {
        _payloadHasher = payloadHasher;
        _options = options ?? new IdempotencyKeyOptions();
        _options.Validate();
    }

    /// <summary>
    /// Build an idempotency key from an operation name and payload parts.
    /// Format: {namespace}:{operation}:{version}:{hash}
    /// </summary>
    public string BuildKey(string operation, params object?[] parts)
    {
        if (string.IsNullOrWhiteSpace(operation))
            throw new ArgumentException("Operation name cannot be empty", nameof(operation));

        var payload = new { operation, parts };
        var hash = _payloadHasher.ComputeCanonicalJsonHash(payload);

        var key = $"{_options.Namespace}:{operation}:{_options.Version}:{hash}";

        return TruncateIfNeeded(key);
    }

    /// <summary>
    /// Build an idempotency key from an operation and a single JSON object.
    /// </summary>
    public string BuildKeyFromJson(string operation, string json)
    {
        if (string.IsNullOrWhiteSpace(operation))
            throw new ArgumentException("Operation name cannot be empty", nameof(operation));

        if (string.IsNullOrWhiteSpace(json))
            throw new ArgumentException("JSON cannot be empty", nameof(json));

        var hash = _payloadHasher.ComputeCanonicalJsonHashFromJson(json);
        var key = $"{_options.Namespace}:{operation}:{_options.Version}:{hash}";

        return TruncateIfNeeded(key);
    }

    /// <summary>
    /// Get the configured options for this builder.
    /// </summary>
    public IdempotencyKeyOptions Options => _options;

    private string TruncateIfNeeded(string key)
    {
        if (key.Length <= _options.MaxKeyLength)
            return key;

        // Truncate and add marker
        var maxHashLength = _options.MaxKeyLength - 3; // Reserve 3 chars for "..."
        var prefix = key[..maxHashLength];
        return $"{prefix}...";
    }

    /// <summary>
    /// Legacy method for backward compatibility.
    /// Use BuildKey() for new code.
    /// </summary>
    public string FromParts(string operation, params object?[] parts) =>
        BuildKey(operation, parts);
}
