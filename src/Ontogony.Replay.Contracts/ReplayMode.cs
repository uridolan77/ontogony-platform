namespace Ontogony.Replay.Contracts;

/// <summary>Shared replay mode vocabulary (REPLAY-RUNTIME-001).</summary>
public static class ReplayMode
{
    /// <summary>Mode value: <c>exact_replay</c>.</summary>
    public const string ExactReplay = "exact_replay";

    /// <summary>Mode value: <c>deterministic_simulation</c>.</summary>
    public const string DeterministicSimulation = "deterministic_simulation";

    /// <summary>Mode value: <c>dry_run</c>.</summary>
    public const string DryRun = "dry_run";

    /// <summary>Mode value: <c>reconstructed</c>.</summary>
    public const string Reconstructed = "reconstructed";

    /// <summary>Mode value: <c>evidence_only</c>.</summary>
    public const string EvidenceOnly = "evidence_only";

    /// <summary>Mode value: <c>unavailable</c>.</summary>
    public const string Unavailable = "unavailable";

    /// <summary>All known replay mode values.</summary>
    public static readonly IReadOnlySet<string> All = new HashSet<string>(StringComparer.Ordinal)
    {
        ExactReplay,
        DeterministicSimulation,
        DryRun,
        Reconstructed,
        EvidenceOnly,
        Unavailable,
    };
}
