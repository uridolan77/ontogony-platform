# SYSTEM-COH-001 — Ontogony Runtime Cohesion Baseline

## Purpose

This package is a Cursor-ready development package for a deep cross-repo cohesion sprint across:

- `allagma-dotnet` — governed runtime and owner of the canonical system lock/matrices.
- `kanon-dotnet` — semantic authority, decision/provenance, policy, human gates, semantic graph.
- `conexus-dotnet` — model gateway, aliases, provider routing, model-call telemetry/fallback.
- `ontogony-platform` — shared mechanics, contracts, schemas, tracing/error/idempotency primitives.
- `ontogony-frontend` — operator console, Evidence Spine workbench, route coverage/operator proof.
- `ontogony-ui` — shared operator UI primitives, status/error/evidence components where needed.

The package is intentionally not a greenfield rewrite. Current repos already contain substantial SYSTEM-COH work: matrices, runtime lock, model-purpose aliasing, streaming purpose support, trace/context matrix, E2E helper scripts, package-mode proof path, Evidence Spine enriched roots, and real-tool blocking docs. The job is to consolidate, verify, promote, and close the runtime cohesion baseline.

## Core thesis

Make Kanon, Conexus, Allagma, Platform, and the operator console behave as one observable, idempotent, policy-governed alpha runtime without violating ownership boundaries.

```text
Ontogony.Platform = shared mechanics and contracts
Conexus          = model access and routing authority
Kanon            = semantic, policy, decision, and provenance authority
Allagma          = governed execution/runtime coordinator
Frontend/UI      = operator inspection, workbench, evidence, and coverage visibility
```

## Success definition

SYSTEM-COH-001 is accepted when a developer can prove, from one coherent acceptance record, that:

1. The runtime lock/matrices match current repo reality.
2. All required ports, auth modes, route prefixes, package refs, and expected smoke commands are declared.
3. The local stack can complete the governed loop Allagma → Kanon → Conexus.
4. Human gate pause/resume is exercised.
5. Kanon → Conexus assistance is exercised with redaction and `draft_only` decision provenance.
6. Conexus fallback is exercised through a system-level path.
7. Trace/correlation/run/model/decision identifiers can be followed across services and into Evidence Spine/operator console artifacts.
8. Idempotent retry does not duplicate durable side effects.
9. Restart/replay evidence is preserved.
10. Real tool execution remains blocked until the trust model is separately accepted.
11. Optional/deferral items are classified explicitly and do not masquerade as failures.

## Package layout

- `00_UNPACK_PROMPT.md` — paste into Cursor at the target repo workspace.
- `01_CURRENT_STATE_BASIS.md` — current-state assumptions and stale-gap warnings.
- `02_SCOPE_AND_NON_GOALS.md` — what this package includes and excludes.
- `03_IMPLEMENTATION_SEQUENCE.md` — phased implementation plan.
- `04_ACCEPTANCE_CRITERIA.md` — hard pass/fail gate.
- `05_REPO_TASKS/` — repo-specific instructions.
- `06_TARGET_ARTIFACTS/` — draft target docs/scripts/schemas to create or merge.
- `07_TEST_SCENARIOS/` — E2E scenario catalogue and evidence expectations.
- `08_CURSOR_PROMPTS/` — smaller phase prompts for iterative execution.
- `09_VALIDATION/` — command map and merge gates.

## How to use

1. Unpack this zip at the workspace root, preferably near `C:\dev\` or wherever sibling repos live.
2. Start with `00_UNPACK_PROMPT.md` in Cursor.
3. Let Cursor audit current repo state first. Do not blindly overwrite existing SYSTEM-COH files.
4. Apply the target artifacts by merging with current docs/scripts/tests.
5. Run the validation commands and save evidence under the platform/allagma evidence artifact paths described in this package.

## Important warning

Some older reviews identified gaps that have since been implemented: Allagma model-purpose aliasing, compatibility matrices, trace/context matrix, streaming purpose support, runtime lock, and E2E helper scripts appear already present. Treat those as baseline assets to validate and harden, not as missing greenfield work.
