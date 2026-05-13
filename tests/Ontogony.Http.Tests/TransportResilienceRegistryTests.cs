using Xunit;
using Microsoft.Extensions.DependencyInjection;

namespace Ontogony.Http.Tests;

/// <summary>
/// Tests for <see cref="TransportResilienceRegistry"/>.
/// </summary>
public class TransportResilienceRegistryTests
{
    [Fact]
    public void Register_StoresOptionsUnderName()
    {
        var registry = new TransportResilienceRegistry();
        var options = new TransportResilienceOptions { MaxRetries = 5 };
        
        registry.Register("my-client", options);
        
        Assert.True(registry.TryGet("my-client", out var retrieved));
        Assert.NotNull(retrieved);
        Assert.Equal(5, retrieved.MaxRetries);
    }

    [Fact]
    public void TryGet_NonExistentName_ReturnsFalse()
    {
        var registry = new TransportResilienceRegistry();
        
        var result = registry.TryGet("nonexistent", out var retrieved);
        
        Assert.False(result);
        Assert.Null(retrieved);
    }

    [Fact]
    public void Register_OverwritesPreviousEntry()
    {
        var registry = new TransportResilienceRegistry();
        var options1 = new TransportResilienceOptions { MaxRetries = 3 };
        var options2 = new TransportResilienceOptions { MaxRetries = 10 };
        
        registry.Register("client", options1);
        registry.Register("client", options2);
        
        Assert.True(registry.TryGet("client", out var retrieved));
        Assert.Equal(10, retrieved.MaxRetries);
    }

    [Fact]
    public void Register_MultipleNames_RetrievesCorrectly()
    {
        var registry = new TransportResilienceRegistry();
        var options1 = new TransportResilienceOptions { MaxRetries = 2 };
        var options2 = new TransportResilienceOptions { MaxRetries = 5 };
        var options3 = new TransportResilienceOptions { MaxRetries = 8 };
        
        registry.Register("client-a", options1);
        registry.Register("client-b", options2);
        registry.Register("client-c", options3);
        
        Assert.True(registry.TryGet("client-a", out var a));
        Assert.True(registry.TryGet("client-b", out var b));
        Assert.True(registry.TryGet("client-c", out var c));
        
        Assert.Equal(2, a.MaxRetries);
        Assert.Equal(5, b.MaxRetries);
        Assert.Equal(8, c.MaxRetries);
    }

    [Fact]
    public void Register_CaseSensitive()
    {
        var registry = new TransportResilienceRegistry();
        var options = new TransportResilienceOptions { MaxRetries = 3 };
        
        registry.Register("MyClient", options);
        
        Assert.True(registry.TryGet("MyClient", out _));
        Assert.False(registry.TryGet("myclient", out _));
        Assert.False(registry.TryGet("MYCLIENT", out _));
    }

    [Fact]
    public void DI_Integration_CanResolveFromContainer()
    {
        var services = new ServiceCollection();
        services.AddSingleton<TransportResilienceRegistry>();
        
        var provider = services.BuildServiceProvider();
        var registry = provider.GetRequiredService<TransportResilienceRegistry>();
        
        Assert.NotNull(registry);
    }
}
