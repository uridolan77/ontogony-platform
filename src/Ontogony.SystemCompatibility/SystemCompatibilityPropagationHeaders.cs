using Ontogony.Http;

namespace Ontogony.SystemCompatibility;

/// <summary>
/// Frozen cross-service propagation headers enforced by PLATFORM-9-001 / PLATFORM-9-003.
/// </summary>
public static class SystemCompatibilityPropagationHeaders
{
    /// <inheritdoc cref="OntogonyPropagationHeaderContract.FrozenRequired"/>
    public static IReadOnlyList<string> Required => OntogonyPropagationHeaderContract.FrozenRequired;
}
