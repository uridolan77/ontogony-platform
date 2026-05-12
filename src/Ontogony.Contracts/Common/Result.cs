namespace Ontogony.Contracts.Common;

/// <summary>
/// Lightweight success/failure carrier without exceptions (opaque error codes/messages).
/// </summary>
/// <typeparam name="T">Value type on success.</typeparam>
/// <param name="Succeeded">True when <see cref="Value"/> is valid.</param>
/// <param name="Value">Success payload when <paramref name="Succeeded"/> is true.</param>
/// <param name="ErrorCode">Opaque machine-readable error code when failed.</param>
/// <param name="ErrorMessage">Human-readable error detail when failed.</param>
public sealed record Result<T>(bool Succeeded, T? Value, string? ErrorCode = null, string? ErrorMessage = null)
{
    /// <summary>Successful result with a value.</summary>
    public static Result<T> Ok(T value) => new(true, value);

    /// <summary>Failed result with code and message.</summary>
    public static Result<T> Fail(string code, string message) => new(false, default, code, message);
}
