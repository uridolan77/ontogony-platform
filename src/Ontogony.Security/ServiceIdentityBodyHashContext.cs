namespace Ontogony.Security;

/// <summary>
/// HttpContext state populated by <see cref="ServiceIdentityBodyHashPreloadMiddleware"/> for bounded async body reads.
/// </summary>
public static class ServiceIdentityBodyHashContext
{
    /// <summary>Key for <see cref="Microsoft.AspNetCore.Http.HttpContext.Items"/> holding <see cref="Precomputed"/>.</summary>
    public static readonly object HttpContextItemKey = new();

    /// <summary>Outcome of middleware body preload.</summary>
    /// <param name="TooLarge">True when the body exceeded <see cref="ServiceIdentityOptions.MaxSignedBodyBytes"/>.</param>
    /// <param name="HexLower">Lowercase hex SHA-256 of captured bytes when hashing succeeded.</param>
    public sealed record Precomputed(bool TooLarge, string? HexLower);
}
