namespace Ontogony.SystemCompatibility;

public sealed record SystemCompatibilityCheck(
    string Id,
    string Title,
    SystemCompatibilityCheckStatus Status,
    string Detail);
