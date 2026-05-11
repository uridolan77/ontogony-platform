using Ontogony.Security;

namespace Ontogony.Testing;

public sealed class FakeCurrentActorAccessor : ICurrentActorAccessor
{
    public CurrentActor? Current { get; set; }
}