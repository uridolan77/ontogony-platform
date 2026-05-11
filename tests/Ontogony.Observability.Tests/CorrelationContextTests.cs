using Ontogony.Observability;
using Xunit;

namespace Ontogony.Observability.Tests;

public sealed class CorrelationContextTests
{
    [Fact]
    public void Push_Restores_Previous_Context_On_Dispose()
    {
        using var outer = OntogonyCorrelationContext.Push("outer");
        Assert.Equal("outer", OntogonyCorrelationContext.TraceId);

        using (OntogonyCorrelationContext.Push("inner"))
        {
            Assert.Equal("inner", OntogonyCorrelationContext.TraceId);
        }

        Assert.Equal("outer", OntogonyCorrelationContext.TraceId);
    }
}
