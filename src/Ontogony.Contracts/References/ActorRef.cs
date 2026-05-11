namespace Ontogony.Contracts.References;

public sealed record ActorRef(
    string ActorId,
    string ActorType,
    string? DisplayName = null,
    string? TenantId = null);
