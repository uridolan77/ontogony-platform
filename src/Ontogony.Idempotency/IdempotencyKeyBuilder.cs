using Ontogony.Hashing;
using System.Security.Cryptography;
using System.Text;

namespace Ontogony.Idempotency;

/// <summary>
/// Constructs deterministic idempotency key strings from canonical JSON fingerprints.
/// Keys are opaque to this library; deduplication semantics live in <see cref="IIdempotencyLedger"/> implementations.
/// </summary>
public sealed class IdempotencyKeyBuilder
{
    private readonly PayloadHasher _payloadHasher;
    private readonly IdempotencyKeyOptions _options;

    /// <summary>Creates a builder with optional formatting options.</summary>
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
        ValidateOperation(operation);

        var payload = new { operation, parts };
        var hash = _payloadHasher.ComputeCanonicalJsonHash(payload);
        return ComposeKey(operation, hash);
    }

    /// <summary>
    /// Build an idempotency key from an operation and a single JSON object.
    /// </summary>
    public string BuildKeyFromJson(string operation, string json)
    {
        ValidateOperation(operation);

        if (string.IsNullOrWhiteSpace(json))
            throw new ArgumentException("JSON cannot be empty", nameof(json));

        var hash = _payloadHasher.ComputeCanonicalJsonHashFromJson(json);
        return ComposeKey(operation, hash);
    }

    /// <summary>
    /// Get the configured options for this builder.
    /// </summary>
    public IdempotencyKeyOptions Options => _options;

    private void ValidateOperation(string operation)
    {
        if (string.IsNullOrWhiteSpace(operation))
            throw new ArgumentException("Operation name cannot be empty", nameof(operation));

        if (!_options.ValidateSafeCharacters)
        {
            return;
        }

        foreach (var ch in operation)
        {
            if (!char.IsLetterOrDigit(ch) && ch != '.' && ch != '-' && ch != '_')
            {
                throw new ArgumentException(
                    $"operation contains unsafe character '{ch}'. Only alphanumeric, dots, hyphens, and underscores allowed.",
                    nameof(operation));
            }
        }
    }

    private string ComposeKey(string operation, string payloadHash)
    {
        var key = $"{_options.Namespace}:{operation}:{_options.Version}:{payloadHash}";
        if (key.Length <= _options.MaxKeyLength)
        {
            return key;
        }

        // Keep the payload hash intact and shrink only the operation component if needed.
        var operationToken = ComputeOperationToken(operation);
        var compactKey = $"{_options.Namespace}:{operationToken}:{_options.Version}:{payloadHash}";
        if (compactKey.Length <= _options.MaxKeyLength)
        {
            return compactKey;
        }

        var fixedLength = _options.Namespace.Length + _options.Version.Length + payloadHash.Length + 3;
        var availableOperationLength = _options.MaxKeyLength - fixedLength;
        if (availableOperationLength < 1)
        {
            throw new InvalidOperationException(
                $"MaxKeyLength ({_options.MaxKeyLength}) is too small to preserve namespace, version, and payload hash.");
        }

        var shortenedOperation = operationToken[..Math.Min(operationToken.Length, availableOperationLength)];
        return $"{_options.Namespace}:{shortenedOperation}:{_options.Version}:{payloadHash}";
    }

    private static string ComputeOperationToken(string operation)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(operation));
        return Convert.ToHexStringLower(bytes)[..12];
    }

    /// <summary>
    /// Legacy method for backward compatibility.
    /// Use BuildKey() for new code.
    /// </summary>
    public string FromParts(string operation, params object?[] parts) =>
        BuildKey(operation, parts);
}
