namespace Ontogony.Secrets;

/// <summary>
/// Mechanical port for resolving a scheme-specific secret reference to a UTF-8 secret string without logging raw values (PR-PLAT-011).
/// </summary>
public interface ISecretValueResolver
{
    /// <summary>
    /// Attempts to resolve <paramref name="reference"/>; implementations must not log returned secret material.
    /// </summary>
    ValueTask<SecretValueResolveResult> TryResolveAsync(
        SecretValueReference reference,
        CancellationToken cancellationToken = default);
}
