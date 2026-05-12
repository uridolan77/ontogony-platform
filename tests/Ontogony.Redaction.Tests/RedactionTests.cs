using Microsoft.Extensions.Options;
using Ontogony.Redaction;
using Xunit;

namespace Ontogony.Redaction.Tests;

public sealed class RedactionTests
{
    [Fact]
    public void Sensitive_field_is_redacted()
    {
        var redactor = new DefaultRedactor(Options.Create(new RedactionOptions()));
        var result = redactor.RedactField("provider_api_key", "sk-test-123456");

        Assert.True(result.WasRedacted);
        Assert.Equal(RedactionClassification.Secret, result.Classification);
        Assert.NotEqual(result.Original, result.Value);
        Assert.EndsWith("3456", result.Value);
    }

    [Fact]
    public void Non_sensitive_field_passes_through()
    {
        var redactor = new DefaultRedactor(Options.Create(new RedactionOptions()));
        var result = redactor.RedactField("model", "gpt-test");

        Assert.False(result.WasRedacted);
        Assert.Equal("gpt-test", result.Value);
    }
}
