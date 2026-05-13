using System.Diagnostics;

namespace Ontogony.Secrets;

/// <summary>
/// Outcome of <see cref="ISecretValueResolver.TryResolveAsync"/>; when <see cref="IsResolved"/> is <see langword="false"/>, <see cref="Value"/> must be ignored.
/// </summary>
/// <remarks>
/// <see cref="ToString"/> never includes resolved secret material. Do not log <see cref="Value"/> in application code.
/// </remarks>
public readonly struct SecretValueResolveResult : IEquatable<SecretValueResolveResult>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SecretValueResolveResult"/> struct.
    /// </summary>
    public SecretValueResolveResult(bool isResolved, string? value, string? unresolvedReason)
    {
        IsResolved = isResolved;
        _value = value;
        UnresolvedReason = unresolvedReason;
    }

    /// <summary>Whether a secret value was resolved.</summary>
    public bool IsResolved { get; }

    /// <summary>Resolved UTF-8 string material; meaningful only when <see cref="IsResolved"/> is <see langword="true"/>.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public string? Value => _value;

    private readonly string? _value;

    /// <summary>Machine-oriented reason when <see cref="IsResolved"/> is <see langword="false"/>; never contains the secret.</summary>
    public string? UnresolvedReason { get; }

    /// <inheritdoc />
    public override string ToString() =>
        IsResolved
            ? "SecretValueResolveResult { IsResolved = true, Value = <redacted> }"
            : $"SecretValueResolveResult {{ IsResolved = false, UnresolvedReason = {FormatReason(UnresolvedReason)} }}";

    private static string FormatReason(string? reason) =>
        string.IsNullOrEmpty(reason) ? "null" : "\"" + reason.Replace("\"", "\\\"", StringComparison.Ordinal) + "\"";

    /// <inheritdoc />
    public override bool Equals(object? obj) =>
        obj is SecretValueResolveResult other && Equals(other);

    /// <inheritdoc />
    public bool Equals(SecretValueResolveResult other) =>
        IsResolved == other.IsResolved &&
        string.Equals(UnresolvedReason, other.UnresolvedReason, StringComparison.Ordinal) &&
        string.Equals(_value, other._value, StringComparison.Ordinal);

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(IsResolved, UnresolvedReason, _value);

    /// <summary>Equality operator.</summary>
    public static bool operator ==(SecretValueResolveResult left, SecretValueResolveResult right) => left.Equals(right);

    /// <summary>Inequality operator.</summary>
    public static bool operator !=(SecretValueResolveResult left, SecretValueResolveResult right) => !left.Equals(right);
}
