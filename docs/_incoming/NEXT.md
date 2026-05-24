Below is the **dev plan from the current point forward**.

**2026-05-24 update:** `SETTINGS-SECURITY-UX-001` implemented (settings/security UX: credential taxonomy, warning consolidation, actor presets, diagnostics privacy, execution posture on settings). **Next:** `EVAL-EVIDENCE-QUALITY-001`.

The current foundation is good: Agent Interaction has already shifted from “API synthesis” to **Live lookup**, fixture/imported/live source labels are now explicit, and the Docker-live E2E checks were strengthened around tool intents, message stream, and interaction panels.  The provider side is also now much richer: the test expects the live provider panel to show `fake` / `fake.chat`, and the live summary to show `summarize-player-risk` and `gaming-core@0.1.0`.  Platform docs were also updated to match the new “live lookup” terminology. 

# Dev roadmap from here

## Phase 0 — Finish the current test gate

### Goal

Close the last proof gate for:

```text
GOVERNED-FAKE-E2E-001
```

### While the test runs

Wait for:

```powershell
npx playwright test -c playwright.docker-local.config.ts governed-fake-e2e-docker-live
```

### If it passes

Immediately do these:

```text
1. Mark GOVERNED-FAKE-E2E-001 as closed.
2. Preserve the live PASS artifact.
3. Move completed incoming packages to consumed/archive.
4. Start AGENT-INTERACTION-LIVE-001.
```

### If it fails

Classify by bucket:

```text
Failure: fixture replay visible
Bucket: Agent Interaction live fallback bug

Failure: live summary missing
Bucket: Agent Interaction live summary projection

Failure: provider panel missing fake/fake.chat
Bucket: Conexus-to-Agent Interaction projection

Failure: Evidence Spine graph partial
Bucket: Evidence resolver / graph completeness

Failure: run creation fails
Bucket: governed-run or contract problem

Failure: health/readiness problem
Bucket: SYSTEM-TRUTH / service runtime, not Agent Interaction
```

Do **not** broadly reopen SYSTEM-TRUTH or Conexus routing unless the failure clearly proves a regression there.

---

# Phase 1 — Close and preserve `GOVERNED-FAKE-E2E-001`

## 1.1 Add evidence summary doc

Create:

```text
ontogony-platform/docs/evidence/GOVERNED_FAKE_E2E_001_PASS_20260523T232255Z.md
```

or, if you prefer per-service evidence:

```text
allagma-dotnet/docs/evidence/GOVERNED_FAKE_E2E_001_PASS_20260523T232255Z.md
ontogony-platform/docs/evidence/GOVERNED_FAKE_E2E_001_PASS_20260523T232255Z.md
```

### Content

````markdown
# GOVERNED-FAKE-E2E-001 PASS

Date: 2026-05-23 / 2026-05-24
Environment: local Docker stack
Services:
- Kanon: localhost:5081
- Conexus: localhost:5082
- Allagma: localhost:5083
- Frontend: localhost:5175

## Commands

```powershell
powershell -File c:\dev\ontogony-platform\scripts\smoke\system_truth_smoke.ps1
powershell -File c:\dev\allagma-dotnet\scripts\smoke\run-governed-fake-e2e.ps1
npx playwright test -c playwright.docker-local.config.ts governed-fake-e2e-docker-live
````

## IDs

* runId:
* traceId:
* correlationId:
* planningDecisionId:
* modelCallId:
* routeDecisionId:
* provider: fake
* providerModel: fake.chat

## Verdict

* Allagma run completed
* Kanon decision resolved by trace
* Conexus model call completed
* Route decision resolved through admin endpoint
* Evidence Spine graph passed
* Agent Interaction opened live lookup, not fixture replay

## Known caveats

* Local fake-provider mode
* Not production IAM
* Runtime lock/CI promotion still separate

````

## 1.2 Update platform index

Update whichever file currently tracks evidence/package state:

```text
ontogony-platform/docs/README.md
ontogony-platform/docs/evidence/README.md
ontogony-platform/docs/system/...
````

Add:

```text
GOVERNED-FAKE-E2E-001 — passed locally; evidence artifact recorded.
```

## 1.3 Archive consumed packages

Move or mark consumed:

```text
docs/_incoming/SYSTEM-TRUTH-001
docs/_incoming/SYSTEM-TRUTH-001A
docs/_incoming/CONEXUS-ROUTING-POSTURE-001
docs/_incoming/GOVERNED-FAKE-E2E-001
```

Recommended structure:

```text
docs/_incoming/_consumed/
  SYSTEM-TRUTH-001/
  SYSTEM-TRUTH-001A/
  CONEXUS-ROUTING-POSTURE-001/
  GOVERNED-FAKE-E2E-001/
```

Add a small `CONSUMED.md` inside each:

```markdown
# Consumed package

Status: implemented / superseded / closed
Closed by:
- commit(s):
- evidence:
- smoke:
- E2E:
Remaining follow-ups:
```

Acceptance:

```text
No active Cursor package remains in docs/_incoming unless it is actually next or in progress.
```

---

# Phase 2 — `AGENT-INTERACTION-LIVE-001`

This should be the next formal sprint.

## Goal

Turn Agent Interaction into a **live operator workbench**, not merely a page that proves it is not fixture replay.

Current state is already much better: live lookup terminology, provider panel, message stream, tool intents, and panels are now checked in E2E. The next step is structure, navigability, and missing-data discipline.

## Repo scope

Primary:

```text
ontogony-frontend
```

Secondary:

```text
ontogony-ui, only if reusable neutral components naturally emerge
allagma-dotnet, only if interaction event API lacks required data
conexus-dotnet, only if model-call detail lacks required projection fields
kanon-dotnet, only if decision/provenance detail needs a small lookup endpoint
```

## Work items

### 2.1 Live lookup must be primary and strict

Rules:

```text
If runId is present:
  attempt live lookup.

If live lookup fails:
  show live lookup error.

Never silently fall back to fixture replay when runId is present.
```

Add explicit states:

```text
live_loading
live_loaded
live_partial
live_failed
fixture_loaded
imported_loaded
```

### 2.2 Event grouping

Group timeline into operator sections:

```text
Run lifecycle
Kanon semantic decisions
Conexus model calls
Provider attempts
Tool intents and results
Human gates
Messages
Evidence and replay
Errors / missing links
```

Each group should show:

```text
count
status
first/last timestamp
expand/collapse
```

### 2.3 Event cards

Every event card should show:

```text
event type
service source
timestamp
primary ID
status/tone
operator label
linked evidence
missing-data reason if applicable
raw payload disclosure
```

Avoid dumping raw JSON by default.

### 2.4 Provider panel refinement

The provider panel should show:

```text
Model purpose
Conexus alias
Provider key
Provider model
Route decision ID
Model call ID
Request ID
Execution run ID
Token usage
Cost
Latency
Fallback used
Message availability
```

For every absent field, show a reason:

```text
not_recorded
not_returned_by_api
redacted
not_applicable
lookup_failed
```

### 2.5 Message stream discipline

Message panel should distinguish:

```text
visible message
redacted message
hash-only message
not recorded
execution journal unavailable
stream withheld
```

No raw provider payloads in list view. Raw detail only behind explicit disclosure.

### 2.6 Tool intents and human gates

If already present, standardize display:

```text
Tool intent requested
Tool decision
Tool result
Tool blocked
Human gate requested
Human gate approved
Human gate denied
Human gate expired/stale
```

Each should link to Evidence Spine or the relevant service page.

### 2.7 Evidence links per event

Each event should expose links like:

```text
Open Evidence Spine
Open Kanon decision
Open Conexus model call
Open route decision
Open Allagma run
Copy traceId
Copy correlationId
```

### 2.8 Export bundle truth

Agent Interaction export bundle should include:

```json
{
  "dataSource": "live | fixture | imported",
  "isLiveEvidence": true,
  "lookup": {
    "runId": "...",
    "traceId": "...",
    "correlationId": "..."
  },
  "missing": [],
  "sourceAttempts": [],
  "privacy": {
    "containsRawSecrets": false,
    "redactionApplied": true
  }
}
```

### 2.9 Tests

Add or extend tests:

```text
unit:
  event grouping
  provider summary selector
  missing reason normalization
  export bundle dataSource/isLiveEvidence
  no silent fixture fallback

component:
  live summary card
  provider panel
  source mode banner
  event group rendering

e2e:
  governed fake live lookup
  fixture explicit mode
  imported replay mode
  live lookup failure does not show fixture
```

Acceptance:

```text
Agent Interaction can be used as the primary live investigation page for a governed fake run.
```

---

# Phase 3 — `KANON-CONSOLE-POLISH-001`

## Goal

Make Kanon’s operator console precise, compact, and semantically trustworthy.

## Repo scope

Primary:

```text
ontogony-frontend
kanon-dotnet, only if capability metadata is missing
```

## Work items

### 3.1 Actor panel cleanup

Replace repeated actor prose with a compact reusable card:

```text
Current operator actor: local-operator
Roles sent: Admin
Profile: Local admin/operator
Kanon domain-pack read: granted
Kanon provenance read: granted
```

### 3.2 Wording replacement

Remove:

```text
Allagma defaults
Kanon trusts headers
Gateway health
```

Use:

```text
In local operator mode, the console sends actor context headers to Kanon.
Kanon authorizes requests according to those roles.
```

Use:

```text
Kanon API health
Kanon readiness
```

### 3.3 Capability matrix

Show:

```text
Domain-pack read
Domain-pack validate
Domain-pack load
Domain-pack promote
Domain-pack deprecate
Provenance read
Decision read
Source-binding read/write
```

Each:

```text
granted / denied / unknown
source: live / estimated from local roles
```

### 3.4 Ontology terminology

Normalize:

```text
Configured ontology
Active ontology
Loaded domain pack
Persisted ontology version
```

Always show full ID:

```text
gaming-core@0.1.0
```

Version-only `0.1.0` should be secondary.

### 3.5 Domain-pack lifecycle

Use one lifecycle vocabulary:

```text
draft
validated
reviewed
accepted
loaded / active
deprecated
```

Separate:

```text
packs on disk
persisted versions
active versions
test/generated packs
```

### 3.6 Conexus Assistance boundary

Label assistance as:

```text
draft-only
advisory
requires human review
does not mutate domain packs automatically
```

Acceptance:

```text
Kanon pages no longer feel like backend debug pages; they read like semantic-authority operator pages.
```

---

# Phase 4 — `SOURCE-BINDINGS-POLISH-001` ✅ implemented (2026-05-24)

## Goal

Make source bindings safe, understandable, and reviewable.

## Repo scope

Primary:

```text
ontogony-frontend
```

Secondary:

```text
kanon-dotnet, only if binding validation/test detail APIs need extension
```

## Work items

```text
1. Remove visible runtime/debug errors.
2. Remove “Backend-waiting list APIs” style copy.
3. Correct route labels to /ontology/v0/source-bindings.
4. Remove dangerous sample defaults from create form.
5. Add “Load sample binding” instead of prefilled risky values.
6. Normalize source system/schema/object/field naming.
7. Add confidence range validation: 0.00–1.00.
8. Make target kind explicit:
   - entity property
   - relationship
9. Separate lifecycle status, review status, and test status.
10. Do not show Approve/Reject on already-approved rows.
11. Expose test warning details.
12. Make binding IDs copyable.
13. Improve responsive/card labels.
```

Acceptance:

```text
A human operator can inspect, create, review, and test source bindings without decoding backend implementation jargon.
```

---

# Phase 5 — `SETTINGS-SECURITY-UX-001` ✅ implemented (2026-05-24)

## Goal

Make Settings/security posture precise and non-misleading.

## Repo scope

Primary:

```text
ontogony-frontend
```

Secondary:

```text
ontogony-ui if reusable settings/security components emerge
```

## Work items

```text
1. Credential-source taxonomy:
   not_set
   default_dev
   session
   local_browser
   env_injected
   service_configured
   unknown_legacy

2. Collapse repeated credential warnings.

3. Add actor role presets:
   read-only semantic authority
   local admin/operator
   system service
   custom

4. Label current actor vs historical run actor vs service actor.

5. Add diagnostics export privacy notice.

6. Add diagnostics export privacy metadata:
   containsClientDiagnostics
   containsRawSecrets
   redactionApplied

7. Remove secret-looking examples:
   apiKey
   secret-live-key
   password
   token
   connectionString

8. Add redaction preview before Conexus Assistance calls.

9. Normalize model naming:
   model purpose
   Conexus alias
   provider key
   provider model

10. Clarify execution posture:
   real external blocked
   sandbox disabled
   kill switch local-alpha warning
```

Acceptance:

```text
Settings no longer looks like a warning dump; it becomes a precise local-alpha control panel.
```

---

# Phase 6 — `EVAL-EVIDENCE-QUALITY-001`

## Goal

Stop evaluation from overstating quality when evidence metadata is thin.

## Work items

```text
1. Evaluation rows include provider metadata.
2. Include modelCallId when present.
3. Include routeDecisionId when present.
4. Include token/cost metadata when available.
5. Persist dataset ID and scenario metadata.
6. Add baseline comparison IDs.
7. Distinguish pass/fail/inconclusive.
8. Add drilldown:
   eval → Allagma run → Kanon decision → Conexus model call → Evidence Spine
9. Mark current-list metrics as current-list metrics, not durable analytics.
10. Add at least one failure/inconclusive scenario.
```

Acceptance:

```text
Evaluation dashboard becomes evidence-aware rather than merely pass-count driven.
```

---

# Phase 7 — `RELEASE-READINESS-TRUTH-001`

## Goal

Make release readiness honest.

## Work items

```text
1. Separate generated readiness artifacts from live validation.
2. Fixture/demo data must not count as release-ready evidence.
3. Local-alpha readiness and production readiness must be separate.
4. Add known caveats table.
5. Link readiness claims to:
   - system truth smoke
   - governed fake E2E
   - compatibility artifact
   - operator journey tests
6. Make stale/missing artifacts visible.
7. Add “not assessed” as a valid state.
```

Acceptance:

```text
The console cannot claim readiness from generated or fixture-only evidence.
```

---

# Phase 8 — `RUNTIME-LOCK-CI-GOVERNED-E2E-001`

## Goal

Promote governed fake E2E from local proof to controlled CI/runtime-lock discipline.

## Staged rollout

```text
Stage 1:
  Manual local smoke, documented.

Stage 2:
  Manual GitHub Actions workflow or local Docker CI script.

Stage 3:
  Runtime-lock validation before release candidate.

Stage 4:
  Required integration gate.
```

Do not jump to Stage 4 too early.

Artifacts:

```text
governed-fake-e2e-summary.json
governed-fake-e2e-output.log
evidence-spine-bundle.json
playwright-report
```

Acceptance:

```text
Anyone can run one command and prove the local governed path.
```

---

# Phase 9 — `DOMAIN-SWITCHER-001`

## Goal

Add clean domain switching only after the current `gaming-core@0.1.0` operator experience is polished.

## Work items

```text
1. Domain selector in operator shell.
2. Current domain context:
   ontologyVersionId
   active domain pack
   model-purpose suggestions
3. Domain-pack list:
   disk packs
   persisted versions
   active versions
4. Domain switch affects:
   Kanon ontology filters
   Allagma start-run ontologyVersionId
   source-binding filters
   Evidence Spine domain context
5. Domain should not silently change Conexus provider routing.
6. Conexus aliases remain separate.
```

Acceptance:

```text
Operator can inspect and switch domains without confusing ontology, domain pack, model purpose, and provider route.
```

---

# Execution order

Use this order:

```text
NOW
  0. Wait for governed-fake Docker-live result.
  1. Close GOVERNED-FAKE-E2E artifact.
  2. Archive consumed docs/_incoming packages.

NEXT
  3. EVAL-EVIDENCE-QUALITY-001.

THEN
  4. RELEASE-READINESS-TRUTH-001.
  5. RUNTIME-LOCK-CI-GOVERNED-E2E-001.

LATER
  6. DOMAIN-SWITCHER-001.
```

## Parallelization

Safe parallel lanes:

```text
Lane A:
  GOVERNED-FAKE closure → AGENT-INTERACTION-LIVE

Lane B:
  docs/_incoming cleanup → evidence artifact preservation

Lane C:
  SETTINGS-SECURITY-UX can run parallel with KANON polish if different files are touched

Lane D:
  EVAL and RELEASE should wait until Agent Interaction and evidence artifacts stabilize
```

Avoid doing these in parallel:

```text
AGENT-INTERACTION-LIVE and major Evidence Spine resolver changes
KANON-CONSOLE-POLISH and SOURCE-BINDINGS-POLISH if both touch the same Kanon shared panels
SETTINGS-SECURITY-UX and KANON actor panel cleanup unless carefully coordinated
```

# Immediate next response to test result

When your current Playwright run finishes:

```text
If green:
  close GOVERNED-FAKE-E2E and start Agent Interaction Live.

If red:
  paste the failing test title and first error block.
```

The next dev move depends on that one result.
