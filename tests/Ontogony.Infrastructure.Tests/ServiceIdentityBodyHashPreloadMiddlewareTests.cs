using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Ontogony.Security;
using Xunit;

namespace Ontogony.Infrastructure.Tests;

public sealed class ServiceIdentityBodyHashPreloadMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_WhenContentLengthExceedsMax_SetsTooLargePrecomputed()
    {
        var options = Options.Create(new ServiceIdentityOptions
        {
            RequireHmacSignature = true,
            MaxSignedBodyBytes = 8,
            AllowUnsignedEmptyBody = false
        });

        var nextInvoked = false;
        Task Next(HttpContext ctx)
        {
            nextInvoked = true;
            return Task.CompletedTask;
        }

        var mw = new ServiceIdentityBodyHashPreloadMiddleware(Next, options);
        var ctx = new DefaultHttpContext();
        ctx.Request.Method = "POST";
        ctx.Request.Headers[OntogonyServiceIdentityHeaders.ServiceId] = "svc";
        ctx.Request.Body = new MemoryStream(new byte[20]);
        ctx.Request.ContentLength = 20;

        await mw.InvokeAsync(ctx);

        Assert.True(nextInvoked);
        var pre = Assert.IsType<ServiceIdentityBodyHashContext.Precomputed>(ctx.Items[ServiceIdentityBodyHashContext.HttpContextItemKey]);
        Assert.True(pre.TooLarge);
        Assert.Null(pre.HexLower);
    }

    [Fact]
    public async Task InvokeAsync_WhenBodyWithinMax_SetsHexAndRewindsOriginalStream()
    {
        var options = Options.Create(new ServiceIdentityOptions
        {
            RequireHmacSignature = true,
            MaxSignedBodyBytes = 1024,
            AllowUnsignedEmptyBody = false
        });

        var bodyBytes = Encoding.UTF8.GetBytes("hello");
        var expectedHex = ServiceIdentityHmacSignatureHelper.ComputeBodyHashHexLower(bodyBytes);

        Task Next(HttpContext ctx) => Task.CompletedTask;

        var mw = new ServiceIdentityBodyHashPreloadMiddleware(Next, options);
        var ctx = new DefaultHttpContext();
        ctx.Request.Method = "POST";
        ctx.Request.Headers[OntogonyServiceIdentityHeaders.ServiceId] = "svc";
        var backing = new MemoryStream(bodyBytes, writable: true);
        ctx.Request.Body = backing;
        ctx.Request.ContentLength = bodyBytes.Length;

        await mw.InvokeAsync(ctx);

        var pre = Assert.IsType<ServiceIdentityBodyHashContext.Precomputed>(ctx.Items[ServiceIdentityBodyHashContext.HttpContextItemKey]);
        Assert.False(pre.TooLarge);
        Assert.Equal(expectedHex, pre.HexLower);
        Assert.Equal(0, backing.Position);
    }

    [Fact]
    public async Task InvokeAsync_WhenOrderViolation_AndThrowEnabled_Throws()
    {
        var options = Options.Create(new ServiceIdentityOptions
        {
            RequireHmacSignature = true,
            EnableBodyHashPreloadOrderDiagnostics = true,
            ThrowOnBodyHashPreloadOrderViolation = true
        });

        Task Next(HttpContext ctx) => Task.CompletedTask;

        var mw = new ServiceIdentityBodyHashPreloadMiddleware(Next, options, NullLogger<ServiceIdentityBodyHashPreloadMiddleware>.Instance);
        var ctx = new DefaultHttpContext();
        ctx.Request.Method = "POST";
        ctx.Request.Headers[OntogonyServiceIdentityHeaders.ServiceId] = "svc";
        ctx.SetEndpoint(new Endpoint(static _ => Task.CompletedTask, EndpointMetadataCollection.Empty, "matched"));

        await Assert.ThrowsAsync<InvalidOperationException>(() => mw.InvokeAsync(ctx));
    }

    [Fact]
    public async Task InvokeAsync_WhenOrderViolation_AndThrowDisabled_Continues()
    {
        var options = Options.Create(new ServiceIdentityOptions
        {
            RequireHmacSignature = true,
            EnableBodyHashPreloadOrderDiagnostics = true,
            ThrowOnBodyHashPreloadOrderViolation = false
        });

        var nextCalled = false;
        Task Next(HttpContext ctx)
        {
            nextCalled = true;
            return Task.CompletedTask;
        }

        var mw = new ServiceIdentityBodyHashPreloadMiddleware(Next, options, NullLogger<ServiceIdentityBodyHashPreloadMiddleware>.Instance);
        var ctx = new DefaultHttpContext();
        ctx.Request.Method = "POST";
        ctx.Request.Headers[OntogonyServiceIdentityHeaders.ServiceId] = "svc";
        ctx.SetEndpoint(new Endpoint(static _ => Task.CompletedTask, EndpointMetadataCollection.Empty, "matched"));

        await mw.InvokeAsync(ctx);

        Assert.True(nextCalled);
    }
}
