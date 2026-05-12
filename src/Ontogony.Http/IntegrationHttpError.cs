using System.Text.RegularExpressions;
using Ontogony.Observability;

namespace Ontogony.Http;

/// <summary>
/// Helpers to redact and truncate HTTP error bodies before logging or throwing.
/// </summary>
public static partial class IntegrationHttpError
{
    /// <summary>Default maximum characters retained after redaction.</summary>
    public const int DefaultMaxBodyChars = 512;

    /// <summary>Redacts bearer tokens and common secret patterns, then truncates to <paramref name="maxChars"/>.</summary>
    public static string RedactAndTruncate(string? body, int maxChars = DefaultMaxBodyChars)
    {
        if (string.IsNullOrEmpty(body))
        {
            return string.Empty;
        }

        var sanitized = body;
        sanitized = BearerValue().Replace(sanitized, "Bearer [REDACTED]");
        sanitized = QuotedSecretJson().Replace(sanitized, m => $"\"{m.Groups["k"].Value}\":\"[REDACTED]\"");
        sanitized = KeyEqualsValue().Replace(sanitized, m => $"{m.Groups["k"].Value}=[REDACTED]");

        return sanitized.Length <= maxChars ? sanitized : sanitized[..maxChars] + "...";
    }

    /// <summary>Throws <see cref="HttpRequestException"/> when <paramref name="response"/> is not successful.</summary>
    public static async Task ThrowIfUnsuccessfulAsync(
        HttpResponseMessage response,
        string integrationLabel,
        CancellationToken cancellationToken)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var raw = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        var safeBody = RedactAndTruncate(raw);
        var traceId = OntogonyCorrelationContext.TraceId;
        var traceSuffix = string.IsNullOrWhiteSpace(traceId) ? string.Empty : $" TraceId={traceId}";
        var message = $"{integrationLabel} HTTP {(int)response.StatusCode} {response.ReasonPhrase}. Body: {safeBody}{traceSuffix}";

        throw new HttpRequestException(message, null, response.StatusCode);
    }

    [GeneratedRegex(@"(?i)Bearer\s+[^\s\r\n""']+", RegexOptions.Compiled)]
    private static partial Regex BearerValue();

    [GeneratedRegex(
        @"(?i)""(?<k>token|apiKey|api_key|password|secret|authorization|bearer)""\s*:\s*""[^""]*""",
        RegexOptions.Compiled)]
    private static partial Regex QuotedSecretJson();

    [GeneratedRegex(
        @"(?i)(?<k>\b(?:token|apikey|api_key|password|secret|authorization)\b)\s*[=:]\s*[^\s&;,\r\n]+",
        RegexOptions.Compiled)]
    private static partial Regex KeyEqualsValue();
}
