function Get-LastExitCode {
    if (Test-Path 'variable:global:LASTEXITCODE') {
        return [int]$global:LASTEXITCODE
    }
    return 0
}

function Resolve-ConformanceRepoRoot {
    param([string]$RepoRoot)
    if ([string]::IsNullOrWhiteSpace($RepoRoot)) {
        $platformRoot = Join-Path $PSScriptRoot '..'
        return (Resolve-Path (Join-Path $platformRoot '..')).Path
    }
    return (Resolve-Path $RepoRoot).Path
}

function Resolve-ConformanceOutputDirectory {
    param(
        [string]$RepoRoot,
        [string]$OutputDirectory,
        [string]$CheckName
    )
    if ([string]::IsNullOrWhiteSpace($OutputDirectory)) {
        $stamp = Get-Date -Format 'yyyyMMddTHHmmssZ'
        return Join-Path $RepoRoot "artifacts/platform-mechanics-conformance/$CheckName/$stamp"
    }
    return $OutputDirectory
}

function Write-ConformanceResult {
    param(
        [string]$OutputDirectory,
        [string]$FileName,
        [object]$Result,
        [string]$Status
    )
    New-Item -ItemType Directory -Force -Path $OutputDirectory | Out-Null
    $Result | ConvertTo-Json -Depth 10 | Set-Content (Join-Path $OutputDirectory $FileName) -Encoding UTF8
    switch ($Status) {
        'PASS' { exit 0 }
        'FAIL' { exit 1 }
        'NOT_RUN' { exit 3 }
        default { exit 2 }
    }
}

function Get-MechanicalSchemaPath {
    param([string]$RepoRoot, [string]$SchemaFileName)
    return Join-Path $RepoRoot "schemas/mechanics/v1/$SchemaFileName"
}

function Test-JsonMatchesMechanicalSchema {
    param(
        [string]$RepoRoot,
        [string]$SchemaFileName,
        [string]$FixturePath
    )
    $testProject = Join-Path $RepoRoot 'tests/Ontogony.SystemCompatibility.Tests/Ontogony.SystemCompatibility.Tests.csproj'
    if (-not (Test-Path -LiteralPath $testProject)) {
        throw "Missing test project for schema validation: $testProject"
    }

    $env:ONTOGONY_MECH_SCHEMA = Get-MechanicalSchemaPath -RepoRoot $RepoRoot -SchemaFileName $SchemaFileName
    $env:ONTOGONY_MECH_FIXTURE = $FixturePath
    try {
        dotnet test $testProject -c Release --filter "FullyQualifiedName~MechanicalSchemaRegistryTests.Validate_fixture_against_schema" --nologo -v q
        return (Get-LastExitCode) -eq 0
    }
    finally {
        Remove-Item Env:ONTOGONY_MECH_SCHEMA -ErrorAction SilentlyContinue
        Remove-Item Env:ONTOGONY_MECH_FIXTURE -ErrorAction SilentlyContinue
        Remove-Item Env:ONTOGONY_MECH_EXPECT_INVALID -ErrorAction SilentlyContinue
    }
}
