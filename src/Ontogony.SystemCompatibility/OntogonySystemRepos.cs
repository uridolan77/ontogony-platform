namespace Ontogony.SystemCompatibility;

/// <summary>
/// Required companion repository names for runtime lock and post-lock delta registers.
/// </summary>
internal static class OntogonySystemRepos
{
    public static readonly string[] RequiredCompanionRepos =
    [
        "allagma-dotnet",
        "ontogony-platform",
        "kanon-dotnet",
        "conexus-dotnet",
        "ontogony-frontend",
        "ontogony-ui"
    ];

    public static readonly HashSet<string> AllowedPostLockClassifications = new(StringComparer.Ordinal)
    {
        "additive_safe_api",
        "operator_doc_only",
        "test_only",
        "runtime_behavior_change",
        "lock_impacting_package",
        "follow_up_needed",
        "validated_alpha_006",
        "moving_main_delta"
    };

    public static readonly HashSet<string> AllowedPostLockDispositions = new(StringComparer.Ordinal)
    {
        "validated_alpha_006",
        "moving_main_delta",
        "no_delta"
    };
}
