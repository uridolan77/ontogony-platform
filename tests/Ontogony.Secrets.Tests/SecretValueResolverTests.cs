using Ontogony.Secrets;
using Xunit;

namespace Ontogony.Secrets.Tests;

public sealed class SecretValueResolverTests
{
    [Fact]
    public void Resolve_result_ToString_never_contains_secret_material()
    {
        var r = new SecretValueResolveResult(true, "super-secret-api-key", null);
        var text = r.ToString();
        Assert.DoesNotContain("super-secret", text, StringComparison.Ordinal);
        Assert.Contains("redacted", text, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task Composite_returns_unresolved_when_entire_chain_fails()
    {
        var composite = new CompositeSecretValueResolver(new ISecretValueResolver[] { new NeverResolver(), new NeverResolver() });
        var result = await composite.TryResolveAsync(new SecretValueReference("env", "ANY_MISSING"));
        Assert.False(result.IsResolved);
        Assert.Equal("unresolved", result.UnresolvedReason);
    }

    [Fact]
    public async Task Environment_resolver_reads_env_var()
    {
        const string name = "ONTOGONY_SECRET_VALUE_RESOLVER_TESTS__X";
        Environment.SetEnvironmentVariable(name, "abc123");
        try
        {
            var resolver = new EnvironmentVariableSecretValueResolver();
            var result = await resolver.TryResolveAsync(new SecretValueReference("env", name));

            Assert.True(result.IsResolved);
            Assert.Equal("abc123", result.Value);
        }
        finally
        {
            Environment.SetEnvironmentVariable(name, null);
        }
    }

    [Fact]
    public async Task Environment_resolver_returns_unresolved_for_unknown_scheme()
    {
        var resolver = new EnvironmentVariableSecretValueResolver();
        var result = await resolver.TryResolveAsync(new SecretValueReference("vault", "ignored"));

        Assert.False(result.IsResolved);
        Assert.Equal("scheme_not_supported", result.UnresolvedReason);
    }

    [Fact]
    public async Task Composite_uses_first_resolving_entry()
    {
        var inner = new EnvironmentVariableSecretValueResolver();
        var never = new NeverResolver();
        var composite = new CompositeSecretValueResolver(new ISecretValueResolver[] { never, inner });

        const string name = "ONTOGONY_SECRET_VALUE_RESOLVER_TESTS__Y";
        Environment.SetEnvironmentVariable(name, "v");
        try
        {
            var result = await composite.TryResolveAsync(new SecretValueReference("env", name));
            Assert.True(result.IsResolved);
            Assert.Equal("v", result.Value);
        }
        finally
        {
            Environment.SetEnvironmentVariable(name, null);
        }
    }

    private sealed class NeverResolver : ISecretValueResolver
    {
        public ValueTask<SecretValueResolveResult> TryResolveAsync(
            SecretValueReference reference,
            CancellationToken cancellationToken = default) =>
            ValueTask.FromResult(new SecretValueResolveResult(false, null, "skip"));
    }
}
