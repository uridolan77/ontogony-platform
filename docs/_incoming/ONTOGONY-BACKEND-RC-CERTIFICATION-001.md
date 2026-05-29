# Backend full-RC plan

## Executive position

The backend is close to RC, but it is **not yet full RC**. The correct next move is not more feature expansion. It is **evidence closure, contract alignment, live-certification hardening, package-mode proof, and zero-silent-skip regression discipline**.

Current backend posture:

| Repo          | Current state                                                                                                                       |
| ------------- | ----------------------------------------------------------------------------------------------------------------------------------- |
| **Allagma**   | Strong alpha. Phenomenological bridge drift is closed; full Allagma test suite reported green at **1327 passed, 1 skipped**.        |
| **Conexus**   | Strong alpha gateway. Cache-metrics blocker closed; gateway hardening wrapper with Postgres is now PASS.                            |
| **Kanon**     | Strong semantic authority. ReplayTarget package surface exists structurally, but package-mode proof still needs explicit closeout.  |
| **Metabole**  | Required SLOD data-spine alpha participant; production readiness not claimed.                                                       |
| **Aisthesis** | Reconstructability spine partial; live five-service PASS still not proven from committed evidence.                                  |
| **Platform**  | Runtime port lock now exists: Kanon 5081, Conexus 5082, Allagma 5083, Metabole 5084, Aisthesis 5085.                                |

The RC plan should be run as one parent program:

```text
ONTOGONY-BACKEND-RC-CERTIFICATION-001
```

Goal:

```text
Move the backend from “strong alpha / RC-certification partial”
to “full backend RC candidate with reproducible evidence.”
```

---

# 1. Full RC definition

Backend RC means all of these are true at the same time:

```text
1. Every backend repo builds and tests green in Release.
2. Package-mode gates pass, especially Allagma consuming Kanon/Conexus/Ontogony packages.
3. Runtime port identity is verified against the canonical lock.
4. Five-service stack boots with Kanon, Conexus, Allagma, Metabole, and Aisthesis.
5. Allagma produces a trace observed by Aisthesis.
6. Metabole produces a schema-profile/SLOD trace observed by Aisthesis.
7. Conexus emits provider/model-call evidence and decision events into the reconstructability path.
8. Kanon emits semantic decision/provenance/replay evidence.
9. System cohesion acceptance passes without silent skips.
10. RC closeout contains machine-readable evidence, human-readable closeout, and deferral index.
11. Production readiness is still explicitly not claimed unless separately certified.
```

No RC pass with hidden assumptions. The platform validation plan already says not to mark parent packages PASS with silent skips. 

---

# 2. RC workstreams

## WS0 — Evidence truth freeze

**Purpose:** stop moving targets before the RC run.

Create a parent evidence folder:

```text
ontogony-platform/docs/evidence/ONTOGONY_BACKEND_RC_CERTIFICATION_001/
```

Required files:

```text
00_RC_SCOPE.md
01_REPO_REFS.json
02_PORT_LOCK_EVIDENCE.json
03_REPO_TEST_MATRIX.json
04_PACKAGE_MODE_MATRIX.json
05_FIVE_SERVICE_LIVE_CERT.json
06_SYSTEM_COHESION_ACCEPTANCE.json
07_DEFERRAL_INDEX.json
08_RC_CLOSEOUT.md
```

Acceptance:

```text
- Every repo commit SHA is recorded.
- Every command is recorded with timestamp, working directory, command line, exit code, and artifact path.
- Every skip has a named owner, blocker, and future package.
- No generic “environment issue” without detail.
```

---

## WS1 — Runtime port and service identity lock

**Current state:** platform contract exists and defines the correct ports.  The verifier script also exists and checks expected identity on 5081–5085. 

**Remaining work:** run it live and commit evidence.

Commands:

```powershell
cd C:\dev\ontogony-platform

pwsh -File .\scripts\system\verify-ontogony-runtime-service-identity.ps1 `
  -WorkspaceRoot C:\dev `
  -RequireAll `
  -WriteEvidence
```

RC acceptance:

```text
Kanon      5081 → pass
Conexus    5082 → pass
Allagma    5083 → pass
Metabole   5084 → pass
Aisthesis  5085 → pass
overallStatus = PASS
```

If this fails, do not run deeper live certs. Port identity mismatch invalidates downstream evidence.

---

## WS2 — Repo-local green gates

Each repo must have a fresh local Release test baseline.

### Allagma

Already recently reported green after phenomenological bridge sync:

```text
dotnet test Allagma.sln -c Release → 1327 passed, 1 skipped
```



Still rerun for RC:

```powershell
cd C:\dev\allagma-dotnet
dotnet restore Allagma.sln
dotnet build Allagma.sln -c Release --no-restore
dotnet test Allagma.sln -c Release --no-build
```

Acceptance:

```text
0 failed
skip list documented
OpenAPI / route inventory / event vocabulary stable
```

### Conexus

Current state records CI-equivalent baseline:

```text
1041 passed, 1 skipped, 0 failed
```



Rerun:

```powershell
cd C:\dev\conexus-dotnet
dotnet restore Conexus.sln
dotnet build Conexus.sln -c Release --no-restore
dotnet test Conexus.sln -c Release --no-build `
  --filter "Category!=ExternalProviderSmoke&Category!=LoadSoak&Category!=PersistenceSmoke&Category!=CapacityBaseline"
```

Acceptance:

```text
0 failed
cache metrics stable
Postgres idempotency acceptance remains green
external provider RC may remain NOT_RUN only if documented as secrets-gated
```

### Kanon

Kanon current state is semantically strong, with 120 `/ontology/v0` routes and 95 client routes.  But its test-baseline section appears older and should be refreshed. 

Rerun:

```powershell
cd C:\dev\kanon-dotnet
dotnet restore Kanon.sln
dotnet build Kanon.sln -c Release --no-restore
dotnet test Kanon.sln -c Release --no-build
```

Acceptance:

```text
0 failed
ReplayTarget package surface tests pass
CURRENT_STATE.md updated with fresh 2026-05-29/30 baseline
```

### Metabole

Metabole remains clean alpha and required SLOD spine. 

Rerun:

```powershell
cd C:\dev\metabole-dotnet
dotnet restore Metabole.sln
dotnet build Metabole.sln -c Release --no-restore
dotnet test Metabole.sln -c Release --no-build
pwsh -File .\scripts\test\run-status-truth.ps1
pwsh -File .\scripts\test\run-undeniability.ps1
```

Acceptance:

```text
0 failed
status truth PASS
undeniability PASS
```

### Aisthesis

Aisthesis is the main RC risk. It needs repo-local green and live-cert diagnostics green.

Commands:

```powershell
cd C:\dev\aisthesis-dotnet
dotnet restore Aisthesis.sln
dotnet build Aisthesis.sln -c Release --no-restore
dotnet test Aisthesis.sln -c Release --no-build
```

Acceptance:

```text
0 failed
live-cert diagnostics scripts present
no-trace failure mode explicitly classified
producer observation summary generated
```

---

## WS3 — Package-mode gates

This is mandatory for RC. A backend RC cannot depend only on sibling-source builds.

### Kanon package-mode ReplayTarget proof

The structural fix exists: `Kanon.Contracts` exposes replay runtime DTOs using `ReplayTarget`, `ReplayEligibility`, and `ReplayEvidenceReference`.  There is also a package-surface test for those types. 

Now prove package-mode.

Commands:

```powershell
cd C:\dev\kanon-dotnet
dotnet pack src\Kanon.Contracts\Kanon.Contracts.csproj -c Release
dotnet pack src\Kanon.Client\Kanon.Client.csproj -c Release
dotnet test Kanon.sln -c Release -p:UseOntogonyPackages=true
```

Then in Allagma:

```powershell
cd C:\dev\allagma-dotnet
pwsh -File .\scripts\run-package-mode-build.ps1
```

Acceptance:

```text
Kanon.Contracts pack PASS
Kanon.Client pack PASS
Allagma package-mode build PASS
No missing ReplayTarget / replay DTO types
```

Evidence file:

```text
kanon-dotnet/docs/evidence/KANON_PACKAGE_MODE_REPLAYTARGET_FIX_001_CLOSEOUT.md
allagma-dotnet/docs/evidence/ALLAGMA_PACKAGE_MODE_RC_PROOF_001.md
```

---

## WS4 — Five-service stack boot

Run from the canonical stack owner. Prefer one script that starts all required services and writes a service inventory.

Recommended command:

```powershell
cd C:\dev\allagma-dotnet
pwsh -File .\scripts\run-five-service-stack.ps1 -DevRoot C:\dev
```

Then verify identity:

```powershell
cd C:\dev\ontogony-platform
pwsh -File .\scripts\system\verify-ontogony-runtime-service-identity.ps1 `
  -WorkspaceRoot C:\dev `
  -RequireAll `
  -WriteEvidence
```

Acceptance:

```text
all services ready
all service identities correct
ports match ONTOGONY_RUNTIME_PORT_LOCK_V1
Conexus provider bootstrap does not block /ready
Postgres available where required
```

---

## WS5 — Aisthesis live producer certification

This is the central RC gate.

The previous failure was:

```text
Metabole trigger → 502
Allagma trigger → 500
traceId → none
producersObserved → empty
```

The RC plan must force these to close.

### 5.1 Allagma producer path

Required outcome:

```text
Allagma run trigger returns 2xx
response includes traceId
Aisthesis observes producer=allagma
trace has run lifecycle events
trace has Kanon decision refs
trace has Conexus model-call refs
```

Command:

```powershell
cd C:\dev\aisthesis-dotnet

$env:AISTHESIS_LIVE_WORKFLOW_TRIGGER_PROFILE = "allagma-run"
$env:AISTHESIS_LIVE_WORKFLOW_TRIGGER_URL = "http://localhost:5083/allagma/v0/runs"

pwsh -File .\scripts\system\run-aisthesis-reconstructability-spine-001.ps1 `
  -WorkspaceRoot C:\dev
```

If this fails, fix in Allagma, not Aisthesis only.

Allagma must guarantee:

```text
traceId in response
correlation headers propagated
Aisthesis producer enabled by config
terminal evidence batch sent
non-2xx downstream errors converted to evidence, not silent loss
```

### 5.2 Metabole producer path

Required outcome:

```text
Metabole schema-profile trigger returns 2xx
response includes traceId and pipelineRunId
pipeline reaches SLOD candidate generation
Kanon validation edge exists
Conexus advisory edge exists when enabled
Aisthesis observes producer=metabole
```

Command:

```powershell
cd C:\dev\aisthesis-dotnet

$env:AISTHESIS_LIVE_WORKFLOW_TRIGGER_PROFILE = "metabole-pipeline"
$env:AISTHESIS_LIVE_WORKFLOW_TRIGGER_URL = "http://localhost:5084/metabole/v0/pipeline-runs/schema-profile"

pwsh -File .\scripts\system\run-aisthesis-reconstructability-spine-001.ps1 `
  -WorkspaceRoot C:\dev
```

If this fails, fix in Metabole.

Metabole must guarantee:

```text
traceId emitted
pipelineRunId emitted
Aisthesis edge batch emitted
Kanon handoff decisionId recorded
Conexus advisory remains advisory, not authoritative
```

### 5.3 Full Aisthesis live cert

Final command:

```powershell
cd C:\dev\aisthesis-dotnet

pwsh -File .\scripts\system\run-five-service-live-certification.ps1 -Mode Live
pwsh -File .\scripts\system\run-aisthesis-rc-certification.ps1 -LiveMode Live
```

Acceptance:

```text
overallStatus = PASS
traceId produced
producersObserved contains allagma
producersObserved contains metabole
required edge families observed
no producer trigger returns 500/502
summary JSON committed or referenced from closeout
```

Evidence file:

```text
aisthesis-dotnet/docs/evidence/AISTHESIS_RC_LIVE_CERTIFICATION_001_PASS.md
```

---

## WS6 — Metabole five-service cert

Metabole must independently prove it can participate in the same five-service runtime.

Command:

```powershell
cd C:\dev\metabole-dotnet

pwsh -File .\scripts\smoke\run-metabole-five-service-certification.ps1 `
  -DevRoot C:\dev `
  -RequireLivePeers `
  -RequirePass
```

Acceptance:

```text
Kanon live validation PASS
Conexus advisory PASS
Allagma orchestration/callback PASS
Aisthesis evidence edges PASS
Postgres durability remains PASS if configured
```

Metabole’s own status already says five-service certification is the right gate. 

Evidence file:

```text
metabole-dotnet/docs/evidence/METABOLE_FIVE_SERVICE_RC_CERTIFICATION_001_PASS.md
```

---

## WS7 — System cohesion acceptance

Run the cross-repo acceptance from Allagma and platform.

Commands:

```powershell
cd C:\dev\allagma-dotnet

pwsh -File .\scripts\system\run-system-cohesion-acceptance.ps1 `
  -UseExistingServices `
  -Quick
```

Then full mode:

```powershell
pwsh -File .\scripts\system\run-system-cohesion-acceptance.ps1 `
  -UseExistingServices
```

Platform validators:

```powershell
cd C:\dev\ontogony-platform

pwsh -File .\scripts\validate-cross-service-error-envelope.ps1 -DevRoot C:\dev
dotnet test tests\Ontogony.SystemCompatibility.Tests -c Release
```

Acceptance:

```text
system cohesion acceptance PASS
propagation header docs PASS
route-client parity PASS
OpenAPI parity PASS
runtime lock PASS
cross-service error envelope PASS
correlation chain PASS
```

---

## WS8 — RC closeout and deferral index

Create one parent closeout:

```text
ontogony-platform/docs/evidence/ONTOGONY_BACKEND_RC_CERTIFICATION_001_CLOSEOUT.md
```

Required sections:

```text
1. Verdict
2. Repo refs
3. Runtime port identity result
4. Repo-local test matrix
5. Package-mode matrix
6. Five-service live certification result
7. Aisthesis reconstructability result
8. Metabole data-spine result
9. System cohesion result
10. Open deferrals
11. Non-claims
12. Operator rerun commands
```

Allowed open deferrals for RC:

```text
External provider RC if real provider secrets unavailable
Enterprise IAM/OIDC
Production SLO certification
ProgressPlay production SQL Server signoff
Durable streaming replay
Replay bundle re-execution, if not part of RC target
```

Not allowed as RC deferrals:

```text
repo-local failing tests
package-mode failure
port identity mismatch
Aisthesis no traceId
Allagma producer not observed
Metabole producer not observed
system cohesion acceptance failure
silent skipped live cert
```

---

# 3. Critical path

The shortest valid path to backend RC is:

```text
1. Verify runtime port identity.
2. Refresh repo-local Release test baselines.
3. Prove Kanon package-mode / Allagma package-mode.
4. Start five-service stack.
5. Make Allagma producer visible to Aisthesis.
6. Make Metabole producer visible to Aisthesis.
7. Pass Aisthesis live RC certification.
8. Pass Metabole five-service certification.
9. Pass system cohesion acceptance.
10. Commit parent closeout and machine evidence.
```

Do not reorder this. Aisthesis live certification before port identity proof is unreliable. Package-mode after parent closeout is too late.

---

# 4. Proposed package structure

Use one parent package:

```text
ONTOGONY-BACKEND-RC-CERTIFICATION-001/
```

Suggested files:

```text
00_MASTER_PROMPT.md
01_EXECUTION_PLAN.md
02_ACCEPTANCE_MATRIX.md
03_COMMAND_RUNBOOK.md
04_DEFERRAL_POLICY.md
05_CLOSEOUT_TEMPLATE.md
06_RISK_REGISTER.md
07_REPO_TASKS.md
08_FULL_RERUN_SCRIPT.md
```

Repo task files:

```text
repo-tasks/allagma/ALLAGMA_AISTHESIS_LIVE_PRODUCER_RC.md
repo-tasks/allagma/ALLAGMA_PACKAGE_MODE_RC.md
repo-tasks/conexus/CONEXUS_GATEWAY_HARDENING_RC_CONFIRM.md
repo-tasks/kanon/KANON_REPLAYTARGET_PACKAGE_MODE_RC.md
repo-tasks/metabole/METABOLE_AISTHESIS_EDGE_RC.md
repo-tasks/aisthesis/AISTHESIS_LIVE_CERT_RC.md
repo-tasks/platform/PLATFORM_PORT_LOCK_RC.md
```

Machine evidence target:

```json
{
  "schema": "ontogony-backend-rc-certification/v1",
  "verdict": "PASS | FAIL | PARTIAL",
  "repoRefs": {},
  "portIdentity": {},
  "repoTests": {},
  "packageMode": {},
  "fiveServiceLive": {},
  "systemCohesion": {},
  "deferrals": []
}
```

---

# 5. Go / no-go rules

## Go to RC if

```text
All repo-local tests PASS
All package-mode gates PASS
Port identity PASS
Five-service live cert PASS
Aisthesis observes Allagma and Metabole producers
Metabole five-service cert PASS
System cohesion acceptance PASS
Parent closeout committed
Deferrals are explicit and non-blocking
```

## No-go if

```text
Any repo has failing default Release tests
Kanon package mode fails
Allagma package-mode build fails
Aisthesis live cert has no traceId
producersObserved is empty or missing Allagma/Metabole
Metabole schema-profile returns 500/502
Allagma run trigger returns 500
port identity mismatches runtime lock
system cohesion acceptance fails
```

---

# Final recommendation

Treat the backend as being in **RC closure mode**, not development mode.

The correct next sprint is:

```text
ONTOGONY-BACKEND-RC-CERTIFICATION-001
```

with one objective:

```text
Produce a reproducible, committed, end-to-end evidence bundle proving
that the five-service Ontogony backend works as a coherent governed,
semantic, model-gateway, data-spine, reconstructability system.
```

Most heavy implementation is already done. The remaining work is to make the system prove itself without ambiguity.
