using Ontogony.Errors;
using Xunit;

namespace Ontogony.Infrastructure.Tests;

public sealed class OntogonyErrorJsonPayloadBuilderTests
{
    [Fact]
    public void Build_respects_details_json_key_alias()
    {
        var options = new OntogonyExceptionMappingOptions
        {
            DetailsJsonKey = "detail",
            IncludeInstanceInJson = false
        };
        var error = new ApiError("NotFound", "missing", "trace-1", new { reason = "gone" }, null);

        var dict = OntogonyErrorJsonPayloadBuilder.Build(options, error);

        Assert.Equal("NotFound", dict["code"]);
        Assert.Equal("missing", dict["message"]);
        Assert.Equal("trace-1", dict["traceId"]);
        Assert.True(dict.ContainsKey("detail"));
        Assert.False(dict.ContainsKey("details"));
    }

    [Fact]
    public void Build_omits_instance_when_disabled()
    {
        var options = new OntogonyExceptionMappingOptions { IncludeInstanceInJson = false };
        var error = new ApiError("X", "Y", Instance: "/ontology/v0/x");

        var dict = OntogonyErrorJsonPayloadBuilder.Build(options, error);

        Assert.False(dict.ContainsKey("instance"));
    }

    [Fact]
    public void Build_includes_instance_when_enabled()
    {
        var options = new OntogonyExceptionMappingOptions { IncludeInstanceInJson = true };
        var error = new ApiError("X", "Y", Instance: "/ontology/v0/x");

        var dict = OntogonyErrorJsonPayloadBuilder.Build(options, error);

        Assert.Equal("/ontology/v0/x", dict["instance"]);
    }
}
