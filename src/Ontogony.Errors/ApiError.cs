namespace Ontogony.Errors;

public sealed record ApiError(
    string Code,
    string Message,
    string? TraceId = null,
    object? Details = null);
