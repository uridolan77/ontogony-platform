param(
    [string[]]$Paths = @(),
    [string]$OutDir = "."
)

$ErrorActionPreference = "Stop"
$patterns = @(
    "rawPrompt",
    "rawCompletion",
    "Authorization:\s*Bearer\s+[A-Za-z0-9._-]{12,}",
    "apiKey\s*[:=]",
    "password\s*[:=]",
    "-----BEGIN"
)

$violations = @()
$scanRoots = @($OutDir)
if ($Paths.Count -gt 0) {
    $scanRoots += $Paths
}

foreach ($root in ($scanRoots | Select-Object -Unique)) {
    if (-not (Test-Path $root)) { continue }
    Get-ChildItem $root -Recurse -File -Include *.json, *.md, *.txt, *.log, *.ps1 -ErrorAction SilentlyContinue | ForEach-Object {
        $content = Get-Content $_.FullName -Raw -ErrorAction SilentlyContinue
        if ([string]::IsNullOrWhiteSpace($content)) { return }
        foreach ($pattern in $patterns) {
            if ($content -match $pattern) {
                $violations += "$($_.FullName): matched '$pattern'"
            }
        }
    }
}

if ($violations.Count -gt 0) {
    $violations | ForEach-Object { Write-Error $_ }
    throw "Raw payload leakage scan failed ($($violations.Count) violation(s))."
}

Write-Host "Raw payload leakage scan PASS."
