# 09 — Operator Runbook

## Local implementation verification

Run per repo:

```powershell
dotnet restore
dotnet build -c Release --no-restore
dotnet test -c Release --no-build
```

Run stack-level smoke from `allagma-dotnet`:

```powershell
./scripts/run-local-stack.ps1
./scripts/smoke-first-system.ps1
./scripts/run-system-cohesion-smoke.ps1 -UseExistingServices
```

After eval harness lands:

```powershell
./scripts/run-cross-repo-eval-smoke.ps1 -UseExistingServices
```

Expected output:

```text
artifacts/eval/<timestamp>/eval-summary.json
```

## Triage checklist

### Eval fails but run completes

Check:

```text
Allagma evaluation artifacts
Kanon decision record ids
Conexus route decision id
baseline comparison
metric thresholds
```

### Kanon denies topology

Check:

```text
ontology version
domain pack policy
actor roles
risk level
requested topology
human gate requirement
```

### Conexus route decision missing

Check:

```text
model alias config
provider config
route-decision persistence
chat telemetry writer
model call metadata
```

### Baseline comparison unsafe

Check:

```text
baseline mode is dry-run/model-only
side-effect ledger not touched twice
tool intent execution disabled
human gate state not duplicated
```

## Release evidence checklist

A release candidate must include:

```text
build proof
test proof
cross-repo eval smoke summary
sample audit bundle
sample Kanon topology decision
sample Conexus route decision
sample baseline comparison
observability metric scrape or fallback evidence
known limitations update
```
