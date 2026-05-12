namespace Ontogony.Errors;

/// <summary>
/// Portable API error shape (machine code, message, optional trace and details).
/// </summary>
/// <param name="Code">Opaque machine-readable error code.</param>
/// <param name="Message">Human-readable summary safe for clients.</param>
/// <param name="TraceId">Optional correlation trace id.</param>
/// <param name="Details">Optional structured detail object.</param>
/// <param name="Instance">Optional URI reference for this occurrence.</param>
public sealed record ApiError(
    string Code,
    string Message,
    string? TraceId = null,
    object? Details = null,
    string? Instance = null);
