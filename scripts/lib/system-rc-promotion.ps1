# System RC promotion helpers (Wave 6). Dot-source from promote-system-rc.ps1.

function Get-RepoGitHead {
    param([string]$RepoPath)
    if (-not (Test-Path -LiteralPath (Join-Path $RepoPath ".git"))) {
        throw "Not a git repo: $RepoPath"
    }
    Push-Location $RepoPath
    try {
        $sha = (git rev-parse HEAD).Trim().ToLowerInvariant()
        if ($sha -notmatch '^[0-9a-f]{40}$') {
            throw "Invalid git HEAD in $RepoPath : $sha"
        }
        return $sha
    }
    finally {
        Pop-Location
    }
}

function Get-FileSha256Hex {
    param([string]$Path)
    if (-not (Test-Path -LiteralPath $Path)) {
        return $null
    }
    return (Get-FileHash -LiteralPath $Path -Algorithm SHA256).Hash.ToLowerInvariant()
}

function Get-JsonFromFile {
    param([string]$Path)
    if (-not (Test-Path -LiteralPath $Path)) {
        return $null
    }
    return Get-Content -LiteralPath $Path -Raw | ConvertFrom-Json
}

function Get-LatestArtifactDirectory {
    param(
        [string]$Root,
        [string]$RelativeParent
    )
    $parent = Join-Path $Root $RelativeParent
    if (-not (Test-Path -LiteralPath $parent)) {
        return $null
    }
    $dirs = Get-ChildItem -LiteralPath $parent -Directory -ErrorAction SilentlyContinue |
        Sort-Object LastWriteTimeUtc -Descending
    if ($dirs.Count -eq 0) {
        return $null
    }
    return $dirs[0].FullName
}

function Get-PackageVersionFromJson {
    param([string]$PackageJsonPath)
    $pkg = Get-JsonFromFile $PackageJsonPath
    if ($null -eq $pkg -or [string]::IsNullOrWhiteSpace([string]$pkg.version)) {
        return $null
    }
    return [string]$pkg.version
}

function Get-ProjectVersion {
    param([string]$RepoPath)
    $buildProps = Join-Path $RepoPath "Directory.Build.props"
    if (Test-Path -LiteralPath $buildProps) {
        $match = Select-String -LiteralPath $buildProps -Pattern '<Version>([^<]+)</Version>' | Select-Object -First 1
        if ($null -ne $match) {
            return $match.Matches[0].Groups[1].Value.Trim()
        }
    }
    return $null
}

function ConvertTo-RcLockFileName {
    param([string]$RcId)
    $slug = ($RcId.ToLowerInvariant() -replace '[^a-z0-9]+', '-').Trim('-')
    return "ontogony-$slug.lock.json"
}

function ConvertTo-EvidenceFolderName {
    param([string]$RcId)
    return ($RcId.ToUpperInvariant() -replace '[^A-Z0-9]+', '_').Trim('_')
}

function Build-SixRepoLockDocument {
    param(
        [string]$DevRoot,
        [string]$RepoRoot,
        [hashtable]$UiRc,
        [hashtable]$FrontendRc
    )

    $runtimeLockPath = Join-Path $DevRoot "allagma-dotnet/docs/system/ontogony-runtime.lock.json"
    $runtimeLock = Get-JsonFromFile $runtimeLockPath
    if ($null -eq $runtimeLock) {
        throw "Missing runtime lock: $runtimeLockPath"
    }

    $platformRoot = Join-Path $DevRoot "ontogony-platform"
    $frontendRoot = Join-Path $DevRoot "ontogony-frontend"
    $uiRoot = Join-Path $DevRoot "ontogony-ui"
    $frontendReleaseLockPath = Join-Path $frontendRoot "docs/generated/frontend-release-lock.json"
    $frontendReleaseLock = Get-JsonFromFile $frontendReleaseLockPath
    if ($null -eq $frontendReleaseLock) {
        throw "Missing frontend release lock: $frontendReleaseLockPath (run frontend rc:check and sync-frontend-release-lock.mjs)"
    }

    $baseline = [string]$runtimeLock.baseline
    $platformPkg = Get-ProjectVersion $platformRoot
    if ([string]::IsNullOrWhiteSpace($platformPkg)) {
        $manifestPath = Join-Path $RepoRoot "artifacts/package-manifest.json"
        $manifest = Get-JsonFromFile $manifestPath
        $platformPkg = [string]$manifest.packageVersion
    }
    if ([string]::IsNullOrWhiteSpace($platformPkg)) {
        $platformPkg = "0.3.0-alpha.1"
    }

    $conexusClient = Get-ProjectVersion (Join-Path $DevRoot "conexus-dotnet")
    $kanonClient = Get-ProjectVersion (Join-Path $DevRoot "kanon-dotnet")
    if ([string]::IsNullOrWhiteSpace($conexusClient)) { $conexusClient = [string]$runtimeLock.packageVersions.'Conexus.Client' }
    if ([string]::IsNullOrWhiteSpace($kanonClient)) { $kanonClient = [string]$runtimeLock.packageVersions.'Kanon.Client' }

    $uiPkgVersion = Get-PackageVersionFromJson (Join-Path $uiRoot "package.json")
    $uiTarballSha = [string]$UiRc.tarballSha256
    if ([string]::IsNullOrWhiteSpace($uiTarballSha)) {
        $uiTarballSha = [string]$frontendReleaseLock.packages.'@ontogony/ui'.tarballSha256
    }

    $buildProv = [string]$FrontendRc.buildProvenanceSha256
    if ([string]::IsNullOrWhiteSpace($buildProv)) {
        $buildProv = [string]$frontendReleaseLock.buildProvenanceSha256
    }

    return [ordered]@{
        schema = "ontogony-six-repo-lock-v1"
        baseline = $baseline
        generatedAt = (Get-Date).ToUniversalTime().ToString("o")
        postLockDeltaRegister = "docs/system/ontogony-six-repo-post-lock-deltas.json"
        repos = [ordered]@{
            "ontogony-platform" = [ordered]@{
                commit = Get-RepoGitHead $platformRoot
                packageVersion = $platformPkg
            }
            "conexus-dotnet" = [ordered]@{
                commit = Get-RepoGitHead (Join-Path $DevRoot "conexus-dotnet")
                clientVersion = $conexusClient
            }
            "kanon-dotnet" = [ordered]@{
                commit = Get-RepoGitHead (Join-Path $DevRoot "kanon-dotnet")
                clientVersion = $kanonClient
            }
            "allagma-dotnet" = [ordered]@{
                commit = Get-RepoGitHead (Join-Path $DevRoot "allagma-dotnet")
                apiPrefix = [string]$runtimeLock.apiPrefixes.allagma
            }
            "ontogony-ui" = [ordered]@{
                commit = Get-RepoGitHead $uiRoot
                packageVersion = $uiPkgVersion
                tarballSha256 = $uiTarballSha
            }
            "ontogony-frontend" = [ordered]@{
                commit = Get-RepoGitHead $frontendRoot
                buildProvenanceSha256 = $buildProv
            }
        }
        contracts = [ordered]@{
            openapiSnapshots = [ordered]@{
                allagma = [string]$frontendReleaseLock.openapiSnapshotHashes.allagma
                conexus = [string]$frontendReleaseLock.openapiSnapshotHashes.conexus
                kanon = [string]$frontendReleaseLock.openapiSnapshotHashes.kanon
            }
            routeInventories = [ordered]@{}
            frontendRouteInventory = [string]$frontendReleaseLock.routeInventoryHash
            uiPublicSubpaths = [string]$frontendReleaseLock.uiConsumerManifestHash
            agentInteractionPackage = [ordered]@{
                commit = [string]$frontendReleaseLock.packages.'@ontogony/agent-interaction'.sourceCommit
                tarballSha256 = [string]$frontendReleaseLock.packages.'@ontogony/agent-interaction'.tarballSha256
            }
        }
    }
}

function Build-SystemRcLockDocument {
    param(
        [string]$RcId,
        [string]$DevRoot,
        [string]$RepoRoot,
        [hashtable]$SixRepoLock,
        [hashtable]$UiRc,
        [hashtable]$FrontendRc,
        [hashtable]$RcArtifacts,
        [string]$EvidenceFolderRel
    )

    $runtimeLock = Get-JsonFromFile (Join-Path $DevRoot "allagma-dotnet/docs/system/ontogony-runtime.lock.json")
    $frontendReleaseLockPath = Join-Path $DevRoot "ontogony-frontend/docs/generated/frontend-release-lock.json"
    $frontendReleaseLock = Get-JsonFromFile $frontendReleaseLockPath

    return [ordered]@{
        schema = "ontogony-system-rc-lock-v1"
        baseline = $RcId
        runtimeBaseline = [string]$runtimeLock.baseline
        generatedAt = (Get-Date).ToUniversalTime().ToString("o")
        sixRepoLock = "docs/system/ontogony-six-repo-lock.json"
        repos = $SixRepoLock.repos
        contracts = [ordered]@{
            openapiSnapshots = $SixRepoLock.contracts.openapiSnapshots
            generatedClientHashes = [ordered]@{
                conexus = [string]$frontendReleaseLock.generatedClientHashes.conexus
                kanon = [string]$frontendReleaseLock.generatedClientHashes.kanon
                allagma = [string]$frontendReleaseLock.generatedClientHashes.allagma
            }
            frontendRouteInventory = [string]$SixRepoLock.contracts.frontendRouteInventory
            uiPublicSubpaths = [string]$SixRepoLock.contracts.uiPublicSubpaths
            agentInteractionPackage = $SixRepoLock.contracts.agentInteractionPackage
        }
        rcArtifacts = [ordered]@{
            platformConsumerConformanceSummarySha256 = [string]$RcArtifacts.platformConsumerConformanceSummarySha256
            uiRcTarballSha256 = [string]$UiRc.tarballSha256
            frontendBuildProvenanceSha256 = [string]$FrontendRc.buildProvenanceSha256
            frontendRcSummarySha256 = [string]$FrontendRc.summarySha256
            sixRepoLockSha256 = [string]$RcArtifacts.sixRepoLockSha256
        }
        evidence = [ordered]@{
            folder = $EvidenceFolderRel
            summaryJson = "$EvidenceFolderRel/system-rc-summary.json"
            summaryMd = "$EvidenceFolderRel/system-rc-summary.md"
        }
    }
}

function Write-JsonFile {
    param(
        [string]$Path,
        [object]$Object
    )
    $dir = Split-Path -Parent $Path
    if (-not (Test-Path -LiteralPath $dir)) {
        New-Item -ItemType Directory -Force -Path $dir | Out-Null
    }
    ($Object | ConvertTo-Json -Depth 12) + "`n" | Set-Content -LiteralPath $Path -Encoding utf8NoBOM
}

function Invoke-NpmScript {
    param(
        [string]$RepoPath,
        [string]$ScriptName
    )
    Push-Location $RepoPath
    try {
        npm run $ScriptName
        if ($LASTEXITCODE -ne 0) {
            throw "npm run $ScriptName failed in $RepoPath (exit $LASTEXITCODE)"
        }
    }
    finally {
        Pop-Location
    }
}

function Read-UiRcArtifacts {
    param([string]$UiRoot)
    $artifactDir = Get-LatestArtifactDirectory $UiRoot "artifacts/ui-rc"
    if ($null -eq $artifactDir) {
        throw "No UI RC artifacts under $UiRoot/artifacts/ui-rc"
    }
    $summaryPath = Join-Path $artifactDir "summary.json"
    $summary = Get-JsonFromFile $summaryPath
    if ($null -eq $summary) {
        throw "Missing UI RC summary: $summaryPath"
    }
    return @{
        artifactDir = $artifactDir
        summaryPath = $summaryPath
        summarySha256 = Get-FileSha256Hex $summaryPath
        tarballSha256 = [string]$summary.tarballSha256
        verdict = [string]$summary.verdict
    }
}

function Read-FrontendRcArtifacts {
    param([string]$FrontendRoot)
    $artifactDir = Get-LatestArtifactDirectory $FrontendRoot "artifacts/frontend-rc"
    if ($null -eq $artifactDir) {
        throw "No frontend RC artifacts under $FrontendRoot/artifacts/frontend-rc"
    }
    $summaryPath = Join-Path $artifactDir "summary.json"
    $summary = Get-JsonFromFile $summaryPath
    if ($null -eq $summary) {
        throw "Missing frontend RC summary: $summaryPath"
    }
    $provenancePath = Join-Path $artifactDir "provenance.json"
    if (-not (Test-Path -LiteralPath $provenancePath)) {
        $provenancePath = Join-Path $FrontendRoot "dist/provenance.json"
    }
    $provSha = Get-FileSha256Hex $provenancePath
    return @{
        artifactDir = $artifactDir
        summaryPath = $summaryPath
        summarySha256 = Get-FileSha256Hex $summaryPath
        buildProvenanceSha256 = $provSha
        provenancePath = $provenancePath
        verdict = [string]$summary.verdict
    }
}

function Copy-IfExists {
    param(
        [string]$Source,
        [string]$Destination
    )
    if (Test-Path -LiteralPath $Source) {
        Copy-Item -LiteralPath $Source -Destination $Destination -Force
    }
}

function Write-SystemRcEvidenceBundle {
    param(
        [string]$EvidenceDir,
        [hashtable]$Summary,
        [hashtable]$Paths
    )

    New-Item -ItemType Directory -Force -Path $EvidenceDir | Out-Null

    Write-JsonFile (Join-Path $EvidenceDir "system-rc-summary.json") $Summary

    $md = @(
        "# $($Summary.baseline) - system release candidate summary",
        "",
        "| Field | Value |",
        "| --- | --- |",
        "| Verdict | **$($Summary.verdict)** |",
        "| Generated | $($Summary.generatedAt) |",
        "| Runtime baseline | $($Summary.runtimeBaseline) |",
        "",
        "## Gates",
        "",
        "| Gate | Verdict |",
        "| --- | --- |"
    )
    foreach ($gate in $Summary.gates) {
        $md += "| $($gate.name) | $($gate.verdict) |"
    }
    $md += ""
    $md += "## Six-repo commits"
    $md += ""
    $md += "| Repo | Commit |"
    $md += "| --- | --- |"
    foreach ($prop in $Summary.repos.PSObject.Properties) {
        $md += "| $($prop.Name) | ``$($prop.Value.commit)`` |"
    }
    $md += ""
    ($md -join "`n") + "`n" | Set-Content -LiteralPath (Join-Path $EvidenceDir "system-rc-summary.md") -Encoding utf8NoBOM

    Copy-IfExists $Paths.platformConsumerConformanceSummary (Join-Path $EvidenceDir "platform-consumer-conformance-summary.json")
    Copy-IfExists $Paths.uiRcSummary (Join-Path $EvidenceDir "ui-rc-summary.json")
    Copy-IfExists $Paths.uiTarballSha256 (Join-Path $EvidenceDir "ui-tarball-sha256.txt")
    Copy-IfExists $Paths.frontendRcSummary (Join-Path $EvidenceDir "frontend-rc-summary.json")
    Copy-IfExists $Paths.frontendProvenance (Join-Path $EvidenceDir "frontend-provenance.json")
    Copy-IfExists $Paths.sixRepoLock (Join-Path $EvidenceDir "six-repo-lock.json")
    Copy-IfExists $Paths.postLockDeltas (Join-Path $EvidenceDir "post-lock-deltas.json")
}
