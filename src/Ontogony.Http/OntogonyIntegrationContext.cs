namespace Ontogony.Http;

/// <summary>
/// Optional outbound integration state for the current async flow (idempotency key, actor metadata).
/// </summary>
public static class OntogonyIntegrationContext
{
    private static readonly AsyncLocal<IntegrationOutboundState?> CurrentValue = new();

    /// <summary>Current outbound integration state, if any.</summary>
    public static IntegrationOutboundState? Current => CurrentValue.Value;

    /// <summary>Replaces outbound integration state until the returned scope is disposed.</summary>
    public static IDisposable Push(IntegrationOutboundState state)
    {
        var prior = CurrentValue.Value;
        CurrentValue.Value = state;
        return new PopScope(prior);
    }

    private sealed class PopScope(IntegrationOutboundState? prior) : IDisposable
    {
        public void Dispose() => CurrentValue.Value = prior;
    }
}

/// <summary>
/// Outbound-only integration metadata carried on the async flow.
/// </summary>
/// <param name="IdempotencyKey">Optional idempotency key to emit on the next outbound call.</param>
/// <param name="ActorType">Optional actor type classifier.</param>
/// <param name="ActorRoles">Optional actor roles.</param>
public sealed record IntegrationOutboundState(
    string? IdempotencyKey = null,
    string? ActorType = null,
    IReadOnlyList<string>? ActorRoles = null);
