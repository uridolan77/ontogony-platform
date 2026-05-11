using Ontogony.Hashing;
using Xunit;

namespace Ontogony.Infrastructure.Tests;

public sealed class Sha256ContentHashServiceTests
{
    [Fact]
    public void ComputeUtf8Sha256_Matches_Known_Vector()
    {
        var service = new Sha256ContentHashService();

        var hash = service.ComputeUtf8Sha256("abc");

        Assert.Equal("ba7816bf8f01cfea414140de5dae2223b00361a396177a9cb410ff61f20015ad", hash);
    }
}
