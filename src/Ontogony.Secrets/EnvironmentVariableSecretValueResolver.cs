namespace Ontogony.Secrets;

/// <summary>
/// Resolves <see cref="SecretValueReference"/> values whose scheme is <c>env</c> (case-insensitive) using <see cref="Environment.GetEnvironmentVariable(string)"/>.
/// </summary>
public sealed class EnvironmentVariableSecretValueResolver : ISecretValueResolver
{
    /// <inheritdoc />
    public ValueTask<SecretValueResolveResult> TryResolveAsync(
        SecretValueReference reference,
        CancellationToken cancellationToken = default)
    {
        _ = cancellationToken;
        if (string.IsNullOrWhiteSpace(reference.Scheme) || string.IsNullOrWhiteSpace(reference.Locator))
        {
            return ValueTask.FromResult(new SecretValueResolveResult(false, null, "invalid_reference"));
        }

        if (!string.Equals(reference.Scheme, "env", StringComparison.OrdinalIgnoreCase))
        {
            return ValueTask.FromResult(new SecretValueResolveResult(false, null, "scheme_not_supported"));
        }

        var value = Environment.GetEnvironmentVariable(reference.Locator);
        if (value is null)
        {
            return ValueTask.FromResult(new SecretValueResolveResult(false, null, "environment_variable_missing"));
        }

        return ValueTask.FromResult(new SecretValueResolveResult(true, value, null));
    }
}
