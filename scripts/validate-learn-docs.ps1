#Requires -Version 5.1
<#
.SYNOPSIS
  Validates docs/learn completeness and referenced script paths (SYSTEM-LEARNING-GUIDE-001).

.EXAMPLE
  ./scripts/validate-learn-docs.ps1
#>
Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$repoRoot = Resolve-Path (Join-Path $PSScriptRoot '..')
$learnRoot = Join-Path $repoRoot 'docs/learn'
$failures = [System.Collections.Generic.List[string]]::new()

$requiredGuides = @(
    'INDEX.md',
    '00_START_HERE.md',
    '01_ARCHITECTURE_MAP.md',
    '02_RUN_LOCAL_SYSTEM.md',
    '03_GOVERNED_FAKE_E2E.md',
    '04_SYSTEM_TRUTH_AND_RELEASE_READINESS.md',
    '05_EVIDENCE_SPINE.md',
    '06_AGENT_INTERACTION.md',
    '07_DOMAIN_MODEL_ROUTING_BOUNDARIES.md',
    '08_CONTRACT_DISCIPLINE.md',
    '09_ADD_A_DOMAIN.md',
    '10_ADD_A_PROVIDER_OR_MODEL_ALIAS.md',
    '11_ADD_OR_CHANGE_AN_API_ROUTE.md',
    '12_ADD_A_FRONTEND_PAGE.md',
    '13_ADD_AN_EVALUATION_OR_BASELINE.md',
    '14_DEBUGGING_PLAYBOOK.md',
    '15_UI_CANONICALIZATION_AND_CONSOLE_UX.md',
    'GLOSSARY.md',
    'DOCS_AUDIT_MATRIX.md'
)

foreach ($name in $requiredGuides) {
    $path = Join-Path $learnRoot $name
    if (-not (Test-Path -LiteralPath $path)) {
        $failures.Add("Missing learn guide: docs/learn/$name")
    }
}

$index = Get-Content -LiteralPath (Join-Path $learnRoot 'INDEX.md') -Raw
foreach ($name in $requiredGuides) {
    if ($name -eq 'INDEX.md') { continue }
    if ($index -notmatch [regex]::Escape($name)) {
        $failures.Add("INDEX.md does not link docs/learn/$name")
    }
}

$requiredScripts = @(
    'scripts/start-local-ontogony-system.ps1',
    'scripts/validate-local-ontogony-system.ps1',
    'docker/local-working-system/scripts/start-local-working-system.ps1',
    'docker/local-working-system/scripts/verify-frontend-browser-provenance.ps1'
)

foreach ($rel in $requiredScripts) {
    $path = Join-Path $repoRoot $rel
    if (-not (Test-Path -LiteralPath $path)) {
        $failures.Add("Missing referenced script: $rel")
    }
}

$evidencePath = Join-Path $repoRoot 'docs/evidence/PLATFORM_SYSTEM_LEARNING_GUIDE_001.md'
if (-not (Test-Path -LiteralPath $evidencePath)) {
    $failures.Add('Missing docs/evidence/PLATFORM_SYSTEM_LEARNING_GUIDE_001.md')
}

# Glossary coverage: core terms must appear in GLOSSARY.md
$glossary = Get-Content -LiteralPath (Join-Path $learnRoot 'GLOSSARY.md') -Raw
$terms = @('Kanon', 'Allagma', 'Conexus', 'Evidence Spine', 'runtime lock', 'ToolIntent', 'model alias', 'model purpose')
foreach ($term in $terms) {
    if ($glossary -notmatch [regex]::Escape($term)) {
        $failures.Add("GLOSSARY.md missing term: $term")
    }
}

if ($failures.Count -gt 0) {
    Write-Error ("validate-learn-docs failed:`n - " + ($failures -join "`n - "))
}

Write-Host "validate-learn-docs OK ($($requiredGuides.Count) guides under docs/learn/)"
