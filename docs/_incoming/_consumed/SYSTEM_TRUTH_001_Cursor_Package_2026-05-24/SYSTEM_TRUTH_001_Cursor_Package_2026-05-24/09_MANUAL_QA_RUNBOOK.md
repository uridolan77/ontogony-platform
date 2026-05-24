# 09 — Manual QA Runbook

## Precondition

Run local stack:

```text
Kanon    http://localhost:5081
Conexus  http://localhost:5082
Allagma  http://localhost:5083
Frontend http://localhost:5173 or current Vite port
```

## Step 1 — Raw endpoint checks

Open:

```text
http://localhost:5081/health
http://localhost:5081/ready
http://localhost:5082/health
http://localhost:5082/ready
http://localhost:5083/health
http://localhost:5083/ready
```

Confirm:

- JSON payloads,
- `schemaVersion`,
- service name,
- version,
- detailed readiness checks.

## Step 2 — Home

Open Operator Home.

Confirm:

- no page-level "Live with fixture fallback";
- each service card has connectivity/readiness/contract/version/data source;
- Conexus strict-not-ready has reason;
- no "All services are healthy" if Conexus not ready;
- compatibility state explains artifact/version status.

## Step 3 — Settings

Confirm:

- service URLs visible;
- credential source labels are explicit;
- health contract warnings no longer generic;
- Conexus not-ready reason visible;
- role/actor copy does not contradict current settings.

## Step 4 — Topology readiness

Confirm:

- current implemented edges separate from planned edges;
- trace bridge "ready to test" is not shown as proof;
- edge rows show expected routes and proof/missing reason.

## Step 5 — Release readiness

Confirm:

- fixture-only/generated routes do not count as real release-ready;
- generated artifact is named as generated;
- missing live validation is explicit.

## Step 6 — Smoke script

Run:

```powershell
.\scripts\smoke\system_truth_smoke.ps1
```

or:

```bash
./scripts/smoke/system_truth_smoke.sh
```

Confirm summary matches UI.
