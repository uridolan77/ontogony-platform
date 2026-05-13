namespace Ontogony.Secrets;

/// <summary>
/// Outcome of <see cref="ISecretValueResolver.TryResolveAsync"/>; when <see cref="IsResolved"/> is <see langword="false"/>, <see cref="Value"/> must be ignored.
/// </summary>
public readonly record struct SecretValueResolveResult(bool IsResolved, string? Value, string? UnresolvedReason);
