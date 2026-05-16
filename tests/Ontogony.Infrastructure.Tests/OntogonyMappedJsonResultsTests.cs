using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Ontogony.Errors;
using Xunit;

namespace Ontogony.Infrastructure.Tests;

public sealed class OntogonyMappedJsonResultsTests
{
    [Fact]
    public async Task ApiError_emits_same_keys_as_payload_builder()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddSingleton<IOptions<JsonOptions>>(_ => Options.Create(new JsonOptions()));
        services.AddSingleton<IOptions<OntogonyExceptionMappingOptions>>(_ => Options.Create(new OntogonyExceptionMappingOptions
        {
            DetailsJsonKey = "detail",
            IncludeInstanceInJson = false,
        }));
        var provider = services.BuildServiceProvider();

        var httpContext = new DefaultHttpContext { RequestServices = provider };
        httpContext.Response.Body = new MemoryStream();

        var result = OntogonyMappedJsonResults.ApiError(
            httpContext,
            StatusCodes.Status404NotFound,
            new ApiError("NotFound", "missing", TraceId: "trace-mapped"));

        await result.ExecuteAsync(httpContext);

        httpContext.Response.Body.Position = 0;
        using var document = await JsonDocument.ParseAsync(httpContext.Response.Body);
        var root = document.RootElement;

        Assert.Equal(404, httpContext.Response.StatusCode);
        Assert.Equal("NotFound", root.GetProperty("code").GetString());
        Assert.Equal("missing", root.GetProperty("message").GetString());
        Assert.Equal("trace-mapped", root.GetProperty("traceId").GetString());
        Assert.False(root.TryGetProperty("instance", out _));
    }
}
