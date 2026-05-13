namespace Ontogony.Secrets;

/// <summary>
/// Runs an ordered list of <see cref="ISecretValueResolver"/> instances until one returns <see cref="SecretValueResolveResult.IsResolved"/> <see langword="true"/>.
/// </summary>
/// <remarks>
/// <para><b>Deterministic rules:</b></para>
/// <list type="number">
/// <item><description>Resolvers run in the order supplied to the constructor (materialized once via <see cref="System.Linq.Enumerable.ToArray{TSource}(System.Collections.Generic.IEnumerable{TSource})"/>).</description></item>
/// <item><description>The first resolver that returns <see cref="SecretValueResolveResult.IsResolved"/> <see langword="true"/> wins; later resolvers are not called.</description></item>
/// <item><description>If every resolver returns <see langword="false"/>, the composite returns <c>new SecretValueResolveResult(false, null, "unresolved")</c> regardless of per-resolver <see cref="SecretValueResolveResult.UnresolvedReason"/> values.</description></item>
/// <item><description>Unsupported schemes or missing environment variables are expressed by individual resolvers (for example <c>scheme_not_supported</c>); the composite does not reinterpret them unless a resolver resolves successfully.</description></item>
/// </list>
/// </remarks>
public sealed class CompositeSecretValueResolver : ISecretValueResolver
{
    private readonly IReadOnlyList<ISecretValueResolver> _chain;

    /// <summary>
    /// Initializes a new instance of the <see cref="CompositeSecretValueResolver"/> class.
    /// </summary>
    /// <param name="resolvers">Ordered resolvers; earlier entries take precedence when multiple could resolve the same reference.</param>
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
