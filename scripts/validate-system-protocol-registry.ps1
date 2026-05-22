#!/usr/bin/env pwsh
<#
.SYNOPSIS
  Validates docs/system/system-protocol-registry.json (SYS-PROTOCOL-REGISTRY-001).
.DESCRIPTION
  Structural checks against the registry schema, baseline alignment with
  allagma-dotnet/docs/system/ontogony-runtime.lock.json, and existence of every
  protocol/evidence path under a DevRoot sibling layout (default: parent of this repo).

  Fails on placeholder commits, baseline/lock drift, missing files, or stale
  package version labels vs the runtime lock.
#>
param(
    [string] $RepoRoot = "",
    [string] $DevRoot = "",
    [switch] $SkipSiblingPaths,
    [switch] $SkipLockCrossCheck
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

if ([string]::IsNullOrWhiteSpace($RepoRoot)) {
    $RepoRoot = (Resolve-Path (Join-Path $PSScriptRoot "..")).Path
}

if ([string]::IsNullOrWhiteSpace($DevRoot)) {
    $DevRoot = (Resolve-Path (Join-Path $RepoRoot "..")).Path
}

$registryPath = Join-Path $RepoRoot "docs/system/system-protocol-registry.json"
$schemaPath = Join-Path $RepoRoot "docs/system/schemas/system-protocol-registry.schema.json"

if (-not (Test-Path -LiteralPath $registryPath)) {
    throw "Missing registry: $registryPath"
}
if (-not (Test-Path -LiteralPath $schemaPath)) {
    throw "Missing schema: $schemaPath"
}

$registry = Get-Content -LiteralPath $registryPath -Raw | ConvertFrom-Json

function Require-NonEmptyString([object]$value, [string]$name) {
    if ($null -eq $value -or ($value -isnot [string]) -or [string]::IsNullOrWhiteSpace($value)) {
        throw "Registry validation failed: '$name' must be a non-empty string."
    }
}

function Test-FullGitSha([string]$sha) {
    return $sha -match '^[0-9a-f]{40}$'
}

Require-NonEmptyString $registry.schema "schema"
if ($registry.schema -ne "ontogony-system-protocol-registry-v1") {
    throw "Registry validation failed: schema must be 'ontogony-system-protocol-registry-v1' (found '$($registry.schema)')."
}

Require-NonEmptyString $registry.baseline "baseline"
if ($registry.baseline -notmatch '^SYSTEM-(ALPHA-[0-9]{3}|RC-001[A-Z])$') {
    throw "Registry validation failed: baseline must match SYSTEM-ALPHA-NNN or SYSTEM-RC-001X (found '$($registry.baseline)')."
}

Require-NonEmptyString $registry.generatedAt "generatedAt"
if ($null -eq $registry.runtimeLock) {
    throw "Registry validation failed: runtimeLock is required."
}
Require-NonEmptyString $registry.runtimeLock.ownerRepo "runtimeLock.ownerRepo"
Require-NonEmptyString $registry.runtimeLock.path "runtimeLock.path"

$repoByName = @{}
foreach ($r in @($registry.repos)) {
    Require-NonEmptyString $r.name "repos[].name"
    Require-NonEmptyString $r.role "repos[].role"
    Require-NonEmptyString $r.ref "repos[].ref"
    Require-NonEmptyString $r.commit "repos[].commit"
    if ($r.commit -match 'TBD|PLACEHOLDER' -or -not (Test-FullGitSha $r.commit)) {
        throw "Registry validation failed: repos[$($r.name)].commit must be a 40-char hex SHA (found '$($r.commit)')."
    }
    if ($repoByName.ContainsKey($r.name)) {
        throw "Registry validation failed: duplicate repo name '$($r.name)'."
    }
    $repoByName[$r.name] = $r
}

$lockRepo = [string]$registry.runtimeLock.ownerRepo
$lockRel = [string]$registry.runtimeLock.path
if (-not $repoByName.ContainsKey($lockRepo)) {
    throw "Registry validation failed: runtimeLock.ownerRepo '$lockRepo' is not listed in repos[]."
}

$lockFull = Join-Path (Join-Path $DevRoot $lockRepo) ($lockRel.Replace('/', [IO.Path]::DirectorySeparatorChar))
$lock = $null
if (Test-Path -LiteralPath $lockFull) {
    $lock = Get-Content -LiteralPath $lockFull -Raw | ConvertFrom-Json
}
elseif (-not $SkipLockCrossCheck) {
    throw "Runtime lock not found for cross-check: $lockFull (use -SkipLockCrossCheck only in partial CI without siblings)."
}

if ($null -ne $lock -and -not $SkipLockCrossCheck) {
    if ([string]$lock.baseline -ne [string]$registry.baseline) {
        throw "Registry baseline '$($registry.baseline)' does not match lock baseline '$($lock.baseline)'."
    }

    $lockedKeys = @("ontogony-platform", "conexus-dotnet", "kanon-dotnet", "allagma-dotnet")
    foreach ($key in $lockedKeys) {
        if (-not $repoByName.ContainsKey($key)) {
            throw "Registry repos[] must include lock participant '$key'."
        }
        $expected = [string]$lock.lockedCommits.$key
        $actual = [string]$repoByName[$key].commit
        if ($expected.ToLowerInvariant() -ne $actual.ToLowerInvariant()) {
            throw "Registry repos[$key].commit '$actual' does not match lock lockedCommits.$key '$expected'."
        }
    }

    if ($null -ne $registry.apiPrefixes) {
        foreach ($k in @("allagma", "kanon", "conexus")) {
            $reg = [string]$registry.apiPrefixes.$k
            $lck = [string]$lock.apiPrefixes.$k
            if (-not [string]::IsNullOrWhiteSpace($reg) -and -not [string]::IsNullOrWhiteSpace($lck) -and $reg -ne $lck) {
                throw "Registry apiPrefixes.$k '$reg' does not match lock '$lck'."
            }
        }
    }

    if ($null -ne $lock.packageVersions) {
        foreach ($r in @($registry.repos)) {
            if (-not ($r.PSObject.Properties.Name -contains 'packageVersions')) { continue }
            $pkgList = $r.packageVersions
            if ($null -eq $pkgList) { continue }
            foreach ($entry in @($pkgList)) {
                if ($entry -notmatch '^([^=]+)=(.+)$') { continue }
                $pkg = $Matches[1]
                $ver = $Matches[2]
                $lockVer = $lock.packageVersions.$pkg
                if ($null -ne $lockVer -and [string]$lockVer -ne $ver) {
                    throw "Registry package version $pkg=$ver on $($r.name) does not match lock packageVersions.$pkg='$lockVer'."
                }
            }
        }
    }
}

$missing = [System.Collections.Generic.List[string]]::new()

function Resolve-RegistryPath([string]$ownerRepo, [string]$relativePath) {
    if (-not $repoByName.ContainsKey($ownerRepo)) {
        throw "Unknown ownerRepo '$ownerRepo' for path '$relativePath'."
    }
    $root = Join-Path $DevRoot $ownerRepo
    return Join-Path $root ($relativePath.Replace('/', [IO.Path]::DirectorySeparatorChar))
}

function Has-Property([object]$obj, [string]$name) {
    return $obj.PSObject.Properties.Name -contains $name
}

$allowedAuthorityModes = @(
    "authoritative",
    "draft_only",
    "simulation_only",
    "blocked",
    "local_only",
    "observational",
    "gateway",
    "unknown"
)

$allowedSideEffectLevels = @(
    "none",
    "read_only",
    "evidence_record",
    "semantic_decision",
    "run_state_transition",
    "model_call",
    "local_sandbox_effect",
    "real_external_blocked",
    "unknown"
)

foreach ($p in @($registry.protocols)) {
    Require-NonEmptyString $p.id "protocols[].id"
    Require-NonEmptyString $p.ownerRepo "protocols[$($p.id)].ownerRepo"

    $hasProtocolId = Has-Property $p "protocolId"
    $hasAuthorityMode = Has-Property $p "authorityMode"
    $hasSideEffectLevel = Has-Property $p "sideEffectLevel"

    if ($hasProtocolId -or $hasAuthorityMode -or $hasSideEffectLevel) {
        if (-not ($hasProtocolId -and $hasAuthorityMode -and $hasSideEffectLevel)) {
            throw "Registry validation failed: protocols[$($p.id)] runtime metadata must include protocolId, authorityMode, and sideEffectLevel together."
        }

        Require-NonEmptyString $p.protocolId "protocols[$($p.id)].protocolId"
        Require-NonEmptyString $p.authorityMode "protocols[$($p.id)].authorityMode"
        Require-NonEmptyString $p.sideEffectLevel "protocols[$($p.id)].sideEffectLevel"

        if ($allowedAuthorityModes -notcontains [string]$p.authorityMode) {
            throw "Registry validation failed: protocols[$($p.id)].authorityMode '$($p.authorityMode)' is not allowed."
        }

        if ($allowedSideEffectLevels -notcontains [string]$p.sideEffectLevel) {
            throw "Registry validation failed: protocols[$($p.id)].sideEffectLevel '$($p.sideEffectLevel)' is not allowed."
        }
    }
}

if (-not $SkipSiblingPaths) {
    foreach ($p in @($registry.protocols)) {
        if ($null -eq $p.paths -or @($p.paths).Count -lt 1) {
            throw "Registry validation failed: protocols[$($p.id)].paths must have at least one entry."
        }
        foreach ($rel in @($p.paths)) {
            $full = Resolve-RegistryPath ([string]$p.ownerRepo) ([string]$rel)
            if (-not (Test-Path -LiteralPath $full)) {
                $missing.Add("$($p.ownerRepo):$rel -> $full") | Out-Null
            }
        }
    }

    foreach ($e in @($registry.evidence)) {
        Require-NonEmptyString $e.id "evidence[].id"
        Require-NonEmptyString $e.ownerRepo "evidence[$($e.id)].ownerRepo"
        Require-NonEmptyString $e.path "evidence[$($e.id)].path"
        $full = Resolve-RegistryPath ([string]$e.ownerRepo) ([string]$e.path)
        if (-not (Test-Path -LiteralPath $full)) {
            $missing.Add("evidence $($e.id): $($e.ownerRepo):$($e.path) -> $full") | Out-Null
        }
    }
}

if ($missing.Count -gt 0) {
    Write-Host "System protocol registry path validation failed:" -ForegroundColor Red
    foreach ($m in $missing) { Write-Host "  $m" }
    throw "Missing $($missing.Count) registry path(s). Ensure DevRoot '$DevRoot' contains sibling repos at locked commits."
}

Write-Host "system-protocol-registry.json OK: $registryPath (baseline=$($registry.baseline), DevRoot=$DevRoot)$(if ($SkipSiblingPaths) { ' [SkipSiblingPaths]' } else { '' })"
