namespace Ontogony.AI.Contracts;

/// <summary>
/// Normalized provider or transport error. <see cref="SafeMessage"/> is only as safe as the caller makes it; prefer redaction
/// in the product layer (for example after <c>Ontogony.Redaction</c> exists) before logging or persisting.
/// </summary>
public sealed record LlmProviderError(string ErrorCode, string? ProviderErrorCode, string? SafeMessage, int? HttpStatusCode, bool IsRetryable);
