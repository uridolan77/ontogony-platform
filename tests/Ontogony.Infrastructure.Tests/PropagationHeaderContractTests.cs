using Ontogony.Contracts.Events;
using Ontogony.Http;
using Ontogony.Testing;
using Xunit;

namespace Ontogony.Infrastructure.Tests;

public sealed class PropagationHeaderContractTests
{
    [Fact]
    public void FrozenRequired_headers_match_PLATFORM_9_003_list()
    {
        var expected = new[]
        {
            "traceparent",
            "X-Correlation-ID",
            "X-Ontogony-Actor-Id",
            "X-Ontogony-Actor-Type",
            "X-Ontogony-Actor-Roles",
            "X-Ontogony-Idempotency-Key",
            "X-Allagma-Run-Id",
        };

        Assert.Equal(expected, OntogonyPropagationHeaderContract.FrozenRequired);
    }

    [Fact]
    public void Frozen_constants_align_with_platform_header_types()
    {
        HeaderPropagationConformanceAssertions.AssertFrozenHeaderConstantsAlign();

        Assert.Equal(OntogonyEventHeaders.TraceParent, OntogonyPropagationHeaderContract.FrozenRequired[0]);
        Assert.Equal(OntogonyIntegrationHeaders.LegacyCorrelationId, OntogonyPropagationHeaderContract.FrozenRequired[1]);
        Assert.Equal(OntogonyIntegrationHeaders.ActorId, OntogonyPropagationHeaderContract.FrozenRequired[2]);
        Assert.Equal(OntogonyIntegrationHeaders.IdempotencyKey, OntogonyPropagationHeaderContract.FrozenRequired[5]);
        Assert.Equal("X-Allagma-Run-Id", OntogonyPropagationHeaderContract.AllagmaRunId);
    }
}
