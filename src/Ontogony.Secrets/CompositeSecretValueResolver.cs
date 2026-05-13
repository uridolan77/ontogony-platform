namespace Ontogony.Secrets;

/// <summary>
/// Runs an ordered list of <see cref="ISecretValueResolver"/> instances until one returns <see cref="SecretValueResolveResult.IsResolved"/> <see langword="true"/>.
/// </summary>
public sealed class CompositeSecretValueResolver : ISecretValueResolver
{
    private readonly IReadOnlyList<ISecretValueResolver> _chain;

    /// <summary>
    /// Initializes a new instance of the <see cref="CompositeSecretValueResolver"/> class.
    /// </summary>
    /// <param name="resolvers">Ordered resolvers; earlier entries take precedence.</param>
    public CompositeSecretValueResolver(IEnumerable<ISecretValueResolver> resolvers)
    {
        ArgumentNullException.ThrowIfNull(resolvers);
        _chain = resolvers.ToArray();
    }

    /// <inheritdoc />
    public async ValueTask<SecretValueResolveResult> TryResolveAsync(
        SecretValueReference reference,
        CancellationToken cancellationToken = default)
    {
        foreach (var resolver in _chain)
        {
            var result = await resolver.TryResolveAsync(reference, cancellationToken).ConfigureAwait(false);
            if (result.IsResolved)
            {
                return result;
            }
        }

        return new SecretValueResolveResult(false, null, "unresolved");
    }
}
