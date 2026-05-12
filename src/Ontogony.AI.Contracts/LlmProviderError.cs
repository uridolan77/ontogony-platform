namespace Ontogony.AI.Contracts;

/// <summary>
/// Normalized provider or transport error (safe for logs; callers choose redaction).
/// </summary>
public sealed record LlmProviderError(string ErrorCode, string? ProviderErrorCode, string? SafeMessage, int? HttpStatusCode, bool IsRetryable);
