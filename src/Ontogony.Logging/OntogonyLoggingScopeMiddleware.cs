using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ontogony.Redaction;

namespace Ontogony.Logging;

/// <summary>
/// Adds an Ontogony logging scope to the current request. Use after request tracing has populated correlation context.
/// </summary>
public sealed class OntogonyLoggingScopeMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<OntogonyLoggingScopeMiddleware> _logger;
    private readonly OntogonyLoggingOptions _options;

    public OntogonyLoggingScopeMiddleware(
        RequestDelegate next,
        ILogger<OntogonyLoggingScopeMiddleware> logger,
        IOptions<OntogonyLoggingOptions> options)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _options = options?.Value ?? new OntogonyLoggingOptions();
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!_options.EnableRequestScope)
        {
            await _next(context);
            return;
        }

        var redactor = context.RequestServices.GetService<IRedactor>();
        using (_logger.BeginOntogonyScope(new Dictionary<string, object?>
        {
            [OntogonyLogFields.Operation] = $"{context.Request.Method} {context.Request.Path}",
            [OntogonyLogFields.Component] = "aspnet.request"
        }, _options, redactor))
        {
            await _next(context);
        }
    }
}
