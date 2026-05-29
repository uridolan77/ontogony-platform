param(
  [Parameter(Mandatory=$true)][string]$KanonBaseUrl,
  [Parameter(Mandatory=$true)][string]$AisthesisBaseUrl,
  [Parameter(Mandatory=$true)][string]$AllagmaBaseUrl,
  [string]$OutDir = "artifacts/phenomenological-memory-authority/live"
)
$ErrorActionPreference = "Stop"
New-Item -ItemType Directory -Force -Path $OutDir | Out-Null

Invoke-RestMethod "$KanonBaseUrl/health" | Out-Null
Invoke-RestMethod "$AisthesisBaseUrl/health" | Out-Null
Invoke-RestMethod "$AllagmaBaseUrl/health" | Out-Null

# TODO: wire to deterministic Allagma authority probe run:
# 1. POST /allagma/v0/runs
# 2. poll /allagma/v0/runs/{runId}/phenomenological-projection
# 3. poll /aisthesis/v1/memory/mutations/{mutationId}/authority-status
# 4. assert decisionRecordUrl contains /ontology/v0/decision-records/
# 5. assert callback state is applied/rejected/pending_review
# 6. assert raw scan passes

throw "Live E2E scaffold is not yet wired to the deterministic authority probe run."
