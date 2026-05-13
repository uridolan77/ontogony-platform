namespace Ontogony.Secrets;

/// <summary>
/// Mechanical port for resolving a scheme-specific secret reference to a UTF-8 secret string without logging raw values (PR-PLAT-011).
/// </summary>
/// <remarks>
/// Implementations must not log or throw <see cref="SecretValueResolveResult.Value"/>. Callers must treat <see cref="SecretValueResolveResult"/> as sensitive: its <see cref="SecretValueResolveResult.ToString"/> redacts values but other surfaces (debuggers, dumps) may still expose memory; avoid retaining resolved values longer than necessary.
/// </remarks>
public interface ISecretValueResolver
{
    /// <summary>
    /// Attempts to resolve <paramref name="reference"/>; implementations must not log returned secret material.
    /// </summary>
    ValueTask<SecretValueResolveResult> TryResolveAsync(
        SecretValueReference reference,
        CancellationToken cancellationToken = default);
}
