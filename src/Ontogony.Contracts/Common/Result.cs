namespace Ontogony.Contracts.Common;

public sealed record Result<T>(bool Succeeded, T? Value, string? ErrorCode = null, string? ErrorMessage = null)
{
    public static Result<T> Ok(T value) => new(true, value);
    public static Result<T> Fail(string code, string message) => new(false, default, code, message);
}
