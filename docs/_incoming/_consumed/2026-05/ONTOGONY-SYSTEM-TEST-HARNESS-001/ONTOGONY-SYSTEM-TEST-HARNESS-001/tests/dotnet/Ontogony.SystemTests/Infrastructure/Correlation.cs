namespace Ontogony.SystemTests.Infrastructure;

public static class Correlation
{
    public static string NewScenarioId(string prefix) => $"{prefix}-{DateTimeOffset.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid():N}"[..Math.Min(64, $"{prefix}-{DateTimeOffset.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid():N}".Length)];

    public static void AddStandardHeaders(
        HttpRequestMessage request,
        string scenarioId,
        string? idempotencyKey = null,
        string? traceId = null,
        IReadOnlyDictionary<string, string>? extra = null)
    {
        request.Headers.TryAddWithoutValidation(OntogonyHeaders.CorrelationId, scenarioId);
        request.Headers.TryAddWithoutValidation(OntogonyHeaders.TraceId, traceId ?? scenarioId);
        request.Headers.TryAddWithoutValidation(
            OntogonyHeaders.ActorId,
            Environment.GetEnvironmentVariable("ONTOGONY_TEST_ACTOR_ID") ?? "system-test-harness");
        request.Headers.TryAddWithoutValidation(
            OntogonyHeaders.ActorType,
            Environment.GetEnvironmentVariable("ONTOGONY_TEST_ACTOR_TYPE") ?? "automation");
        var roles = Environment.GetEnvironmentVariable("ONTOGONY_TEST_ACTOR_ROLES") ?? "tester,operator,admin";
        request.Headers.TryAddWithoutValidation(OntogonyHeaders.ActorRoles, roles);
        request.Headers.TryAddWithoutValidation(OntogonyHeaders.LegacyActorRoles, roles);

        if (!string.IsNullOrWhiteSpace(idempotencyKey))
        {
            request.Headers.TryAddWithoutValidation(OntogonyHeaders.IdempotencyKey, idempotencyKey);
            request.Headers.TryAddWithoutValidation(OntogonyHeaders.LegacyIdempotencyKey, idempotencyKey);
        }

        if (extra is null)
        {
            return;
        }

        foreach (var entry in extra)
        {
            request.Headers.TryAddWithoutValidation(entry.Key, entry.Value);
        }
    }
}
