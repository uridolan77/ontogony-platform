namespace Ontogony.Security;

public sealed record CurrentActor(
    string ActorId,
    string ActorType,
    string[] Roles,
    string? TenantId = null,
    string? WorkspaceId = null,
    string? ProjectId = null,
    string? Email = null);

public interface ICurrentActorAccessor
{
    CurrentActor? Current { get; }
}
