using Ontogony.Secrets;
using Xunit;

namespace Ontogony.Secrets.Tests;

public sealed class SecretsTests
{
    [Fact]
    public void Development_protector_round_trips()
    {
        var protector = new DevelopmentBase64SecretProtector();

        var protectedValue = protector.Protect("secret-value", "key-1");

        Assert.Equal(DevelopmentBase64SecretProtector.Scheme, protectedValue.ProtectionScheme);
        Assert.Equal("secret-value", protector.Unprotect(protectedValue.ProtectedValue));
    }

    [Fact]
    public void Fingerprint_is_stable_and_hex()
    {
        var service = new Sha256SecretFingerprintService();

        var a = service.ComputeFingerprint("secret");
        var b = service.ComputeFingerprint("secret");

        Assert.Equal(a, b);
        Assert.Equal(64, a.Length);
        Assert.Matches("^[0-9a-f]{64}$", a);
    }
}
