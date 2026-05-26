# ONTOGONY-SKILL-OPTIMIZATION-SPINE-001

Implementation package for adding a governed **Skill Optimization Spine** across the Ontogony multi-repo system.

The package turns the SkillOpt idea into an Ontogony-native engineering slice: skills are not loose prompt snippets. They become versioned, evaluable, auditable, deployable, semantically governed procedural artifacts.

## Core thesis

Ontogony should treat agent skills as **external trainable state** for frozen models and fixed execution harnesses:

```text
rollout evidence + verifier scores + decision evidence
        ↓
minibatch reflection over successes and failures
        ↓
bounded add/delete/replace skill edits
        ↓
Kanon semantic validation + policy classification
        ↓
held-out evaluation gate
        ↓
accepted skill version or rejected-edit memory
        ↓
controlled deployment through Allagma + Conexus
```

This is not a request to build a generic prompt optimizer. It is a request to add a governed optimization loop where every candidate edit, validation result, rejection reason, promotion decision, and deployment binding is reconstructable.

## Repositories

Expected target repos:

- `ontogony-platform` — protocol docs, package integration, local compose notes, cross-repo validation.
- `allagma-dotnet` — skill-optimization run orchestration, lifecycle, operations, replay, evidence capture.
- `kanon-dotnet` — skill ontology, semantic authority, edit validation, evidence graph, publish/promotion decisions.
- `conexus-dotnet` — optimizer/target model routing, skill injection, model-call metadata, provider safety boundaries.
- `ontogony-frontend` — Skill Lab, run timeline, edit diff, gate results, deployment controls.
- `ontogony-ui` — reusable skill cards, score panels, diff components, warning badges.

## First vertical slice

Build the smallest strong slice that proves the spine end-to-end:

1. Register one demo skill artifact: `demo.spreadsheet-analysis.v0`.
2. Execute a fake-provider rollout batch using the current skill.
3. Generate bounded candidate edits from synthetic failure/success evidence.
4. Apply a deterministic validator and held-out evaluation gate.
5. Accept one improving candidate and reject one regressing candidate.
6. Record both accepted and rejected edits as governed evidence.
7. Publish a validated `SkillVersion` for fixture/local use.
8. Inject the selected skill into a Conexus fake-provider target call.
9. Render the full process in the frontend Skill Lab.

## Non-negotiable rules

- Do not silently rewrite a skill document.
- Do not promote a skill unless a held-out gate passes.
- Do not use hidden model chain-of-thought as evidence.
- Do not add runtime optimizer calls to normal inference.
- Do not make skills global by default; every deployment needs an explicit binding and scope.
- Do not create a parallel evidence universe; link to existing Allagma runs, Conexus calls, Kanon decisions, traces, and evidence spine objects.

## Package contents

- `00_UNPACK_PROMPT.md` — generic Cursor unpack prompt.
- `01_EXECUTIVE_BRIEF.md` — strategic rationale and success criteria.
- `02_TARGET_ARCHITECTURE.md` — cross-service architecture.
- `03_contracts/` — canonical contract specs.
- `04_backend/` — repo-specific backend implementation notes.
- `05_frontend/` — Skill Lab and UI state specs.
- `06_testing/` — acceptance gates, fixtures, harness strategy, contract drift.
- `07_rollout/` — phased execution plan, risks, implementation order.
- `08_source_notes/` — mapping from SkillOpt to Ontogony.
- `10_prompts/` — repo-specific Cursor prompts.
- `templates/` — JSON schema and fixture templates.
- `examples/` — example skill and evaluation artifacts.

## Finish condition

The package is complete only when a local operator can open the console and answer:

```text
Which skill is deployed for this agent profile?
Which version is active?
Why was this version accepted?
Which candidate edits were rejected?
Which runs, traces, model calls, verifier scores, and Kanon decisions support it?
Can I roll back safely?
Will normal inference call the optimizer? No.
```
