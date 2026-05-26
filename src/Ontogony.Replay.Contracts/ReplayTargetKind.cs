namespace Ontogony.Replay.Contracts;

/// <summary>Replay target kinds across Allagma, Kanon, Conexus, and Evidence Spine.</summary>
public static class ReplayTargetKind
{
    /// <summary>Target kind value: <c>allagma.run</c>.</summary>
    public const string AllagmaRun = "allagma.run";

    /// <summary>Target kind value: <c>allagma.audit_bundle</c>.</summary>
    public const string AllagmaAuditBundle = "allagma.audit_bundle";

    /// <summary>Target kind value: <c>allagma.interaction_stream</c>.</summary>
    public const string AllagmaInteractionStream = "allagma.interaction_stream";

    /// <summary>Target kind value: <c>kanon.decision</c>.</summary>
    public const string KanonDecision = "kanon.decision";

    /// <summary>Target kind value: <c>kanon.semantic_plan</c>.</summary>
    public const string KanonSemanticPlan = "kanon.semantic_plan";

    /// <summary>Target kind value: <c>kanon.provenance</c>.</summary>
    public const string KanonProvenance = "kanon.provenance";

    /// <summary>Target kind value: <c>conexus.model_call</c>.</summary>
    public const string ConexusModelCall = "conexus.model_call";

    /// <summary>Target kind value: <c>conexus.route_decision</c>.</summary>
    public const string ConexusRouteDecision = "conexus.route_decision";

    /// <summary>Target kind value: <c>conexus.provider_attempt</c>.</summary>
    public const string ConexusProviderAttempt = "conexus.provider_attempt";

    /// <summary>Target kind value: <c>platform.trace</c>.</summary>
    public const string PlatformTrace = "platform.trace";

    /// <summary>Target kind value: <c>platform.correlation</c>.</summary>
    public const string PlatformCorrelation = "platform.correlation";

    /// <summary>Target kind value: <c>evidence_spine_bundle</c>.</summary>
    public const string EvidenceSpineBundle = "evidence_spine_bundle";

    /// <summary>All known replay target kind values.</summary>
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
