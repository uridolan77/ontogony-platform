using Ontogony.Secrets;
using Xunit;

namespace Ontogony.Secrets.Tests;

public sealed class SecretValueReferenceParserTests
{
    [Theory]
    [InlineData("env:NAME", "env", "NAME")]
    [InlineData("vault:path", "vault", "path")]
    [InlineData(" env:NAME ", "env", "NAME")]
    [InlineData("vault:secret/path:with:colons", "vault", "secret/path:with:colons")]
    public void TryParse_accepts_scheme_colon_locator(string input, string expectedScheme, string expectedLocator)
    {
        var ok = SecretValueReferenceParser.TryParse(input, out var reference, out var error);

        Assert.True(ok);
        Assert.Null(error);
        Assert.Equal(expectedScheme, reference.Scheme);
        Assert.Equal(expectedLocator, reference.Locator);
    }

    [Fact]
    public void TryParse_rejects_missing_colon()
    {
        var ok = SecretValueReferenceParser.TryParse("nocolon", out var reference, out var error);

        Assert.False(ok);
        Assert.Equal(default, reference);
        Assert.Equal("Secret value reference must use scheme:locator form.", error);
    }

    [Fact]
    public void TryParse_rejects_null()
    {
        var ok = SecretValueReferenceParser.TryParse(null, out var reference, out var error);

        Assert.False(ok);
        Assert.Equal(default, reference);
        Assert.Equal("Secret value reference is missing.", error);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void TryParse_rejects_whitespace_or_empty(string value)
    {
        var ok = SecretValueReferenceParser.TryParse(value, out var reference, out var error);

        Assert.False(ok);
        Assert.Equal(default, reference);
        Assert.Equal("Secret value reference is missing.", error);
    }

    [Theory]
    [InlineData(":locator")]
    [InlineData(" :locator ")]
    public void TryParse_rejects_blank_scheme(string value)
    {
        var ok = SecretValueReferenceParser.TryParse(value, out var reference, out var error);

        Assert.False(ok);
        Assert.Equal(default, reference);
        Assert.Equal("Secret value reference scheme must not be blank.", error);
    }

    [Theory]
    [InlineData("env:")]
    [InlineData("vault: ")]
    public void TryParse_rejects_blank_locator(string value)
    {
        var ok = SecretValueReferenceParser.TryParse(value, out var reference, out var error);

        Assert.False(ok);
        Assert.Equal(default, reference);
        Assert.Equal("Secret value reference locator must not be blank.", error);
    }
}
