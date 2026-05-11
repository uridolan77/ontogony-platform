using System.Net;
using Athanor.Application.Exceptions;
using Athanor.Contracts.Common;

namespace Athanor.Api.Middleware;

public sealed class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ArgumentException ex)
        {
            await WriteErrorAsync(
                context,
                HttpStatusCode.BadRequest,
                new ApiError("ValidationFailed", ex.Message, ex.ParamName));
        }
        catch (GeneratedExtractionRejectedException ex)
        {
            await WriteErrorAsync(
                context,
                (HttpStatusCode)ex.HttpStatusCode,
                new ApiError(ex.Code, ex.Message, ex.Detail));
        }
        catch (EntityNotFoundException ex)
        {
            await WriteErrorAsync(
                context,
                HttpStatusCode.NotFound,
                new ApiError("NotFound", ex.Message, ex.EntityName));
        }
        catch (EntityConflictException ex)
        {
            await WriteErrorAsync(
                context,
                HttpStatusCode.Conflict,
                new ApiError(ex.Code, ex.Message, null));
        }
        catch (AuthorityDeniedException ex)
        {
            await WriteErrorAsync(
                context,
                HttpStatusCode.Forbidden,
                new ApiError("AuthorityDenied", ex.Message, null));
        }
        catch (UnauthorizedAccessException ex)
        {
            await WriteErrorAsync(
                context,
                HttpStatusCode.Forbidden,
                new ApiError("AuthorityDenied", ex.Message, null));
        }
        catch (Exception ex)
        {
            var traceId = context.Items.TryGetValue(TraceIdMiddleware.ItemKey, out var v) ? v?.ToString() : null;
            _logger.LogError(ex, "Unhandled request failure (traceId: {TraceId})", traceId);

            await WriteErrorAsync(
                context,
                HttpStatusCode.InternalServerError,
                new ApiError("PersistenceFailure", "An unexpected error occurred.", null));
        }
    }

    private static async Task WriteErrorAsync(
        HttpContext context,
        HttpStatusCode statusCode,
        ApiError error)
    {
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(error);
    }
}
