using System.Text;
using Microsoft.AspNetCore.Http;
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
}
