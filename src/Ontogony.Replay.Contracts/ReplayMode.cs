namespace Ontogony.Replay.Contracts;

/// <summary>Shared replay mode vocabulary (REPLAY-RUNTIME-001).</summary>
public static class ReplayMode
{
    public const string ExactReplay = "exact_replay";
    public const string DeterministicSimulation = "deterministic_simulation";
    public const string DryRun = "dry_run";
    public const string Reconstructed = "reconstructed";
    public const string EvidenceOnly = "evidence_only";
    public const string Unavailable = "unavailable";

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
