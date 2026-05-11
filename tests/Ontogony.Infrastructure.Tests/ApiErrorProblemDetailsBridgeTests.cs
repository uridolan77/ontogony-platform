using Microsoft.AspNetCore.Mvc;
using Ontogony.Errors;
using Xunit;

namespace Ontogony.Infrastructure.Tests;

public sealed class ApiErrorProblemDetailsBridgeTests
{
    [Fact]
    public void ToProblemDetails_Maps_ApiError_Fields()
    {
        var error = new ApiError(
            "ValidationFailed",
            "The request is invalid.",
            "trace-123",
            new { field = "name" },
            "/api/v1/items");

        var problem = error.ToProblemDetails(statusCode: 400);

        Assert.Equal(400, problem.Status);
        Assert.Equal("The request is invalid.", problem.Title);
        Assert.Equal("The request is invalid.", problem.Detail);
        Assert.Equal("/api/v1/items", problem.Instance);
        Assert.Equal("ValidationFailed", problem.Extensions["code"]);
        Assert.Equal("trace-123", problem.Extensions["traceId"]);
        Assert.True(problem.Extensions.ContainsKey("details"));
    }

    [Fact]
    public void ToApiError_Maps_ProblemDetails_Fields()
    {
        var problem = new ProblemDetails
        {
            Status = 403,
            Title = "Forbidden",
            Detail = "Forbidden detail",
            Instance = "/api/v1/secure"
        };
        problem.Extensions["code"] = "Forbidden";
        problem.Extensions["traceId"] = "trace-456";
        problem.Extensions["details"] = new { reason = "policy" };

        var error = problem.ToApiError();

        Assert.Equal("Forbidden", error.Code);
        Assert.Equal("Forbidden", error.Message);
        Assert.Equal("trace-456", error.TraceId);
        Assert.NotNull(error.Details);
        Assert.Equal("/api/v1/secure", error.Instance);
    }

    [Fact]
    public void ToApiError_Uses_Default_Code_When_Missing()
    {
        var problem = new ProblemDetails
        {
            Title = "Unexpected failure"
        };

        var error = problem.ToApiError();

        Assert.Equal("ProblemDetailsError", error.Code);
        Assert.Equal("Unexpected failure", error.Message);
    }
}
