# 10 — Manual QA Runbook

## Prerequisites

Start local services:

```text
Kanon:   http://localhost:5081
Conexus: http://localhost:5082
Allagma: http://localhost:5083
Frontend dev server as usual
```

Confirm:

```text
Real external execution: blocked
Sandbox execution: disabled or explicitly local-only
Conexus fake provider available
Kanon active ontology: gaming-core@0.1.0
Allagma model purpose: summarize-player-risk -> risk-summary-v0
```

## Step 1 — Start governed fake run

Use UI:

```text
Allagma -> Start Run
Preset: summarize player risk
Model purpose: summarize-player-risk
Context: playerId = 123
Start and open Evidence Spine
```

or API script:

```powershell
./scripts/smoke/run-governed-fake-e2e.ps1
```

## Step 2 — Capture IDs

Record:

```text
runId:
traceId:
correlationId:
planningDecisionId:
modelCallId:
routeDecisionId:
```

## Step 3 — Resolve by run id

Open Evidence Spine and paste `runId`.

Expected:

```text
Allagma run resolved
Kanon decision resolved
Conexus model call resolved
Route decision resolved
Provider attempt resolved
Trace/correlation resolved
```

## Step 4 — Resolve by trace id

Paste `traceId`.

Expected: same graph or equivalent graph.

## Step 5 — Resolve by model call id

Paste `modelCallId`.

Expected:

```text
Conexus nodes resolved
Allagma/Kanon expanded if trace/correlation links are available
```

## Step 6 — Check route decision directly

```powershell
Invoke-RestMethod `
  -Method Get `
  -Uri "http://localhost:5082/admin/v0/route-decisions/<routeDecisionId>" `
  -Headers @{ "X-Conexus-Admin-Key" = "cx-conexus-admin-dev" }
```

Expected:

```text
200
providerKey = fake
providerModel = fake.chat
requested/resolved alias visible
```

## Step 7 — Check Kanon decision by trace

```powershell
Invoke-RestMethod `
  -Method Get `
  -Uri "http://localhost:5081/ontology/v0/decision-records/by-trace/<traceId>" `
  -Headers @{ Authorization = "Bearer <kanon token if required>" }
```

Expected:

```text
decision list contains planning decision
```

## Step 8 — Export bundle

Evidence Spine -> Export bundle.

Expected:

```text
schema present
nodes present
source attempts present
no fixture ids
no unexpected route decision error
missing links are absent or accurately not_applicable
```
