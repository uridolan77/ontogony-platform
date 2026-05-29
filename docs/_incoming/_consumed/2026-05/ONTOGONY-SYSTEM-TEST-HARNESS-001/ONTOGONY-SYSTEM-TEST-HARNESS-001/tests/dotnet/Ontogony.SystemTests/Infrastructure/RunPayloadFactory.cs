namespace Ontogony.SystemTests.Infrastructure;

public static class RunPayloadFactory
{
    public const string DefaultOntologyVersionId = "gaming-core@0.1.0";
    public const string HumanGateProbeMarker = "[maf-consequential-probe]";

    public static object GovernedFirstLoop(string actorId = "system-test-harness", string playerId = "456") => new
    {
        ontologyVersionId = DefaultOntologyVersionId,
        actorId,
        actorType = "service",
        actorRoles = new[] { "AgentExecutor", "RiskAnalyst" },
        objective = "ONTOGONY-SYSTEM-TEST-HARNESS E2E-001 governed first-loop.",
        context = new Dictionary<string, object?> { ["playerId"] = playerId }
    };

    public static object IdempotentRun(string actorId, string playerId, string objectiveSuffix) => new
    {
        ontologyVersionId = DefaultOntologyVersionId,
        actorId,
        actorType = "service",
        actorRoles = new[] { "AgentExecutor" },
        objective = $"Idempotency harness check {objectiveSuffix}.",
        context = new Dictionary<string, object?> { ["playerId"] = playerId }
    };

    public static object HumanGateRun(string actorId, string playerId, string traceSuffix) => new
    {
        ontologyVersionId = DefaultOntologyVersionId,
        actorId,
        actorType = "agent",
        actorRoles = new[] { "RiskAnalyst", "PaymentsOperator" },
        objective = $"Summarize risk status for player {playerId}. {HumanGateProbeMarker} trace={traceSuffix}",
        context = new Dictionary<string, object?> { ["playerId"] = playerId }
    };

    public static object KanonResolveHumanGate(string resolveActorId, string resolution) => new
    {
        actor = new
        {
            actorId = resolveActorId,
            actorType = "agent",
            roles = new[] { "PaymentsOperator" }
        },
        resolution,
        notes = (string?)null
    };

    public static object SummarizeContradictionAssistance() => new
    {
        ontologyVersionId = DefaultOntologyVersionId,
        actor = new
        {
            actorId = "system-test-harness",
            actorType = "agent",
            roles = new[] { "Reviewer", "Admin" }
        },
        contradictionId = "sys-test-contradiction-001",
        context = new Dictionary<string, object?>
        {
            ["ticketId"] = "T-100",
            ["apiKey"] = "secret-live-key",
            ["summary"] = "Synthetic contradiction for system-test harness."
        },
        allowedFields = new[] { "ticketId", "apiKey", "summary" },
        redactedFields = new[] { "apiKey" }
    };

    public static object ConexusChatCompletion(string model, string userContent) => new
    {
        model,
        messages = new[]
        {
            new { role = "system", content = "Deterministic test assistant. No tools." },
            new { role = "user", content = userContent }
        },
        stream = false
    };

    public static object ConexusFallbackAliasUpsert(string alias) => new
    {
        alias,
        providerKey = "fake",
        providerModel = "fake.fail.retryable",
        enabled = true,
        priceCatalogVersion = "dev-bootstrap-v0",
        fallbackChain = new[]
        {
            new { providerKey = "fake", providerModel = "fake.echo.fallback", ordinal = 1 }
        }
    };
}
