Below is the **Kanon plan** I would run next. The target is to move Kanon from “strong semantic authority service with many mature surfaces” to a **fully closed semantic value loop** at the same maturity level now reached by Replay, Runtime Config, Console UX, MAF, and Conexus Provider Parity.

Kanon is already in a good state: the operator guide defines the semantic value loop from domain-pack authoring through validation, impact analysis, review queue, promotion/load/deprecation, semantic quality snapshots, and feedback.  Lifecycle work is also far ahead of the original package: lifecycle sidecar, transition POST, frontend transition controls, transition hardening, golden parity, and enriched Evidence Spine artifact IDs are all recorded as done.  The remaining explicit deferred items are assistance conversion, source-binding/domain-pack/review-queue UI consolidation, and full governed-fake E2E acceptance closure. 

# Package name

```text
KANON-SEMANTIC-CLOSURE-001
```

Goal:

```text
Make Kanon’s semantic value loop executable, visible, measurable, and proof-locked end-to-end.
```

Target score:

```text
Kanon current:     ~8.6/10
Kanon target:      9.2–9.4/10
```

---

# Slice 1 — Acceptance closure and drift cleanup

## Goal

Close the remaining acceptance debt from `KANON-SEMANTIC-DEPTH-001/002/003`.

## Work

Run and record:

```text
npm run contracts:discipline
governed fake E2E
Kanon route parity / client coverage
Kanon frontend operator coverage
Kanon OpenAPI baseline update
Kanon route inventory check
```

Update:

```text
kanon-dotnet/docs/_incoming/_active/KANON-SEMANTIC-DEPTH-001/IMPLEMENTATION_NOTES.md
kanon-dotnet/docs/evidence/KANON_SEMANTIC_DEPTH_CLOSURE_001.md
ontogony-platform/docs/learn/09_ADD_A_DOMAIN.md
ontogony-platform/docs/learn/08_CONTRACT_DISCIPLINE.md
```

## Acceptance

```text
- No stale route counts remain in current docs.
- Generated route inventory is canonical.
- contracts:discipline passes.
- Evidence doc records current route/client counts.
- Old KANON-SEMANTIC-DEPTH acceptance checklist is either complete or explicitly superseded.
```

---

# Slice 2 — Assistance conversion without violating authority

## Package

```text
KANON-ASSISTANCE-CONVERT-001
```

## Problem

The deferred route is still important:

```text
POST /ontology/v0/conexus/assistance/{decisionId}/convert
```

Kanon currently keeps assistance advisory, which is correct. The next step is not to make assistance authoritative; it is to convert accepted assistance into a **governed semantic proposal**.

The operator guide already states that Conexus assistance is draft-only and not semantic authority.  The lifecycle notes also preserve this rule: assistance accept stays advisory, not semantic authority. 

## Work

Add conversion route:

```http
POST /ontology/v0/conexus/assistance/{decisionId}/convert
```

Conversion targets:

```text
source_binding_suggestion
canonical_fact_audit
domain_pack_patch
policy_denial_review
semantic_plan_proposal
```

Rules:

```text
- Accepting assistance does not create authority.
- Conversion creates a proposal/review item.
- Converted proposal links back to original assistance decision.
- Converted proposal receives its own lifecycle.
- Converted artifact is visible in Evidence Spine.
```

## Contracts

Add:

```text
AssistanceConversionRequest
AssistanceConversionResponse
AssistanceConversionTargetKind
AssistanceConversionPreview
AssistanceConversionDecisionLink
```

## Frontend

On `/kanon/assistance` and `/kanon/decisions`:

```text
- Show “Convert to semantic proposal” only for eligible accepted assistance drafts.
- Display target type selector.
- Show preview before conversion.
- Link created proposal/review item.
```

## Acceptance

```text
- Assistance remains advisory.
- Convert route creates proposal, not authority.
- Review queue receives conversion-created item.
- Evidence Spine shows:
  assistance decision → conversion decision → proposed semantic artifact.
```

---

# Slice 3 — Review queue as the semantic workbench

## Package

```text
KANON-REVIEW-QUEUE-UNIFICATION-001
```

## Problem

Kanon has many review domains: source bindings, assistance, canonical facts, policy denials, domain packs. The operator guide already presents review queue as the unified work list.  The UI should make it the real semantic workbench.

## Work

Backend:

```text
- Add review item action availability endpoint or enrich existing item detail.
- Normalize review item source/target links.
- Add review resolution effect summary:
  creates_decision
  transitions_lifecycle
  creates_source_binding_review
  creates_fact_audit
  creates_policy_review
  creates_domain_pack_lifecycle_transition
```

Frontend:

```text
/kanon/review-queue:
  default:
    priority queue
    selected item
    primary action
    semantic effect summary

  disclosure:
    raw provenance
    graph edges
    lifecycle transition detail
    assistance/source-binding/domain-pack internals
```

Add one consistent resolution control model:

```text
Resolve
Accept
Reject
Convert
Supersede
Deprecate
Request more context
```

## Acceptance

```text
- Operator can clear all Kanon review item types from one workbench.
- Every resolution either creates or links a decision.
- Lifecycle badges update after action.
- Evidence Spine links are present for every item.
```

---

# Slice 4 — Source-binding and domain-pack loop completion

## Package

```text
KANON-SEMANTIC-VALUE-LOOP-001
```

## Goal

Make the operator loop truly end-to-end:

```text
author domain pack
validate
impact analysis
review queue
promote/load/deprecate
source binding review
quality snapshot
feedback into next iteration
```

## Work

Backend:

```text
- Add semantic loop summary endpoint:
  GET /ontology/v0/semantic-value-loop/status?ontologyVersionId=...
```

Return:

```text
active ontology
active domain pack
open review items
pending source bindings
latest validation report
latest impact report
latest rollback plan
latest quality snapshot
recent lifecycle transitions
blocked promotion reasons
next recommended operator action
```

Frontend:

Add to `/kanon` or `/kanon/domain-packs`:

```text
Semantic value loop panel
  current step
  next action
  blockers
  quality trend
  evidence links
```

## Acceptance

```text
- Operator can see where a domain/ontology stands.
- Domain-pack promotion blockers are visible.
- Quality snapshot and review queue feed back into the same loop.
- No new top-level page unless strictly needed.
```

---

# Slice 5 — Semantic quality gates and regression tracking

## Package

```text
KANON-SEMANTIC-QUALITY-GATES-001
```

## Current baseline

The product guide already places semantic quality snapshots near the end of the value loop and says they feed the next authoring iteration. 

## Work

Backend:

```text
- Add quality baseline concept.
- Add quality comparison between snapshots.
- Add regression classification:
  improved
  stable
  regressed
  insufficient_data
```

Metrics:

```text
source binding coverage
contradiction count
assistance accept/reject ratio
policy denial precision
canonical fact audit outcomes
review queue aging
domain-pack validation blockers
```

Routes:

```http
POST /ontology/v0/semantic-quality/baselines
GET  /ontology/v0/semantic-quality/baselines
POST /ontology/v0/semantic-quality/compare
```

Frontend:

```text
/kanon or /kanon/domain-packs:
  latest quality status
  baseline comparison
  regression warnings
  source metrics
```

## Acceptance

```text
- Quality snapshots are not just one-off metrics.
- Operators can compare current semantic state to a baseline.
- Regressions create review queue items or warnings.
- Evidence Spine links quality snapshots to facts/bindings/domain packs.
```

---

# Slice 6 — Kanon Evidence Spine full closure

## Package

```text
KANON-EVIDENCE-SPINE-CLOSURE-001
```

Evidence Spine enriched roots are already recorded as done for:

```text
canonicalFactId
semanticPlanId
semanticQualitySnapshotId
operatorReviewItemId
ontologyVersionId
sourceBindingId
```



Now close the loop by making each enriched root show the full semantic chain.

## Work

For each root, ensure graph expansion includes:

```text
canonicalFactId:
  fact
  audits
  source bindings
  domain pack
  decisions
  quality snapshots

semanticPlanId:
  plan
  Kanon decision
  Allagma run if present
  tool intents if present
  replay if present

semanticQualitySnapshotId:
  metric rows
  contributing facts/bindings/review items
  baseline comparison if present

operatorReviewItemId:
  source decision
  resolution decision
  target artifact
  lifecycle transition

ontologyVersionId:
  domain packs
  source bindings
  quality snapshots
  review queue state

sourceBindingId:
  binding
  approval history
  quality coverage
  contradictions
  review item
```

## Acceptance

```text
- Every enriched root resolves into useful graph nodes.
- No “dead-end” Kanon evidence roots.
- Graph links back to Allagma/Conexus where relevant.
- Handoff doc and generated entrypoints stay current.
```

---

# Slice 7 — Tool-intent lifecycle strict smoke alignment

## Package

```text
TOOL-INTENT-LIFECYCLE-STRICT-001
```

This is cross-service, but Kanon is central because tool intent authority passes through Kanon.

## Problem

Conexus parity closure explicitly leaves strict `smoke-first-system.ps1` open: without `-SkipToolIntentLifecycleAssert`, the demo run records `ToolIntentBlocked`, not `Allowed`. 

## Work

Decide the intended fixture truth:

Option A — governed fake smoke should demonstrate allow path:

```text
- Adjust fixture/domain/action policy so one deterministic tool intent is allowed.
- Assert ToolIntentAllowed.
- Assert execution path is simulated/safe.
```

Option B — governed fake smoke should demonstrate denial path:

```text
- Rename strict assertion to expect blocked.
- Update smoke language and docs.
- Add separate allow-path smoke.
```

I recommend Option A plus a separate denial smoke.

## Acceptance

```text
- smoke-first-system passes without skip.
- There is one allow-path tool intent smoke.
- There is one denial-path smoke.
- Kanon decision lifecycle and authority effect are explicit in both.
```

---

# Slice 8 — Kanon proof lock

## Package

```text
KANON-SEMANTIC-PROOF-LOCK-001
```

## Goal

Like MAF and Replay, Kanon should have a canonical smoke artifact proving the semantic value loop.

## Smoke flow

```text
1. Load baseline ontology/domain pack.
2. Create authoring session.
3. Validate session.
4. Persist impact report.
5. Submit review item.
6. Resolve review item.
7. Transition lifecycle.
8. Promote/load domain pack.
9. Compute quality snapshot.
10. Resolve Evidence Spine from:
    ontologyVersionId
    sourceBindingId
    operatorReviewItemId
    semanticQualitySnapshotId
11. Verify semantic value loop status endpoint.
```

Artifact:

```text
kanon-semantic-loop-summary.json
kanon-semantic-loop-evidence-bundle.json
kanon-semantic-loop-summary.md
```

Schema:

```text
ontogony-kanon-semantic-loop-summary-v1
```

## Acceptance

```text
- PASS artifact committed under docs/evidence/artifacts.
- Optional runtime-lock entry.
- Manual CI workflow exists.
- Evidence Spine graph confirms semantic loop.
```

---

# Final package sequence

```text
1. KANON-ACCEPTANCE-CLOSURE-001
2. KANON-ASSISTANCE-CONVERT-001
3. KANON-REVIEW-QUEUE-UNIFICATION-001
4. KANON-SEMANTIC-VALUE-LOOP-001
5. KANON-SEMANTIC-QUALITY-GATES-001
6. KANON-EVIDENCE-SPINE-CLOSURE-001
7. TOOL-INTENT-LIFECYCLE-STRICT-001
8. KANON-SEMANTIC-PROOF-LOCK-001
```

## Best immediate first package

Start with:

```text
KANON-ACCEPTANCE-CLOSURE-001
```

It is small, clarifies the current truth, and prevents building the next slices on stale package-state assumptions.

Then do:

```text
KANON-ASSISTANCE-CONVERT-001
```

That is the most important functional Kanon enhancement: it turns Conexus assistance from a useful advisory artifact into a governed semantic proposal pipeline, while preserving the core rule that assistance is not authority.

## Bottom line

Kanon is no longer missing basic lifecycle or Evidence Spine integration. Those are largely done. The next level is to make Kanon’s **semantic value loop** fully executable and proof-locked:

```text
draft → validate → analyze impact → review → convert/resolve → promote/load → measure quality → feed back
```

That is the right Kanon roadmap now.
