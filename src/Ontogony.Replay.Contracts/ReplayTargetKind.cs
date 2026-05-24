namespace Ontogony.Replay.Contracts;

/// <summary>Replay target kinds across Allagma, Kanon, Conexus, and Evidence Spine.</summary>
public static class ReplayTargetKind
{
    public const string AllagmaRun = "allagma.run";
    public const string AllagmaAuditBundle = "allagma.audit_bundle";
    public const string AllagmaInteractionStream = "allagma.interaction_stream";
    public const string KanonDecision = "kanon.decision";
    public const string KanonSemanticPlan = "kanon.semantic_plan";
    public const string KanonProvenance = "kanon.provenance";
    public const string ConexusModelCall = "conexus.model_call";
    public const string ConexusRouteDecision = "conexus.route_decision";
    public const string ConexusProviderAttempt = "conexus.provider_attempt";
    public const string PlatformTrace = "platform.trace";
    public const string PlatformCorrelation = "platform.correlation";
    public const string EvidenceSpineBundle = "evidence_spine_bundle";

    public static readonly IReadOnlySet<string> All = new HashSet<string>(StringComparer.Ordinal)
    {
        AllagmaRun,
        AllagmaAuditBundle,
        AllagmaInteractionStream,
        KanonDecision,
        KanonSemanticPlan,
        KanonProvenance,
        ConexusModelCall,
        ConexusRouteDecision,
        ConexusProviderAttempt,
        PlatformTrace,
        PlatformCorrelation,
        EvidenceSpineBundle,
    };
}
