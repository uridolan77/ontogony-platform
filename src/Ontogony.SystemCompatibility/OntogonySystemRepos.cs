namespace Ontogony.SystemCompatibility;

/// <summary>
/// Required companion repository names for runtime lock and post-lock delta registers.
/// </summary>
internal static class OntogonySystemRepos
{
    /// <summary>Repos required in <c>lockedCommits</c> on the runtime lock.</summary>
    public static readonly string[] RequiredLockedCommitRepos =
    [
        "allagma-dotnet",
        "ontogony-platform",
        "kanon-dotnet",
        "conexus-dotnet"
    ];

    /// <summary>Repos required in post-lock delta <c>repos[]</c> (backend four-pack).</summary>
    public static readonly string[] RequiredPostLockRepos = RequiredLockedCommitRepos;
}
