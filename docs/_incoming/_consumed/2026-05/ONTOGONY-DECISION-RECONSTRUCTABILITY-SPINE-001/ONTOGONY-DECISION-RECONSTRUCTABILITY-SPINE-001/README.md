# ONTOGONY-DECISION-RECONSTRUCTABILITY-SPINE-001

Implementation package for adding a first-class **Decision Reconstructability Spine** across the Ontogony multi-repo system.

This package is motivated by the paper *Property-Level Reconstructability of Agent Decisions: An Anchor-Level Pilot Across Vendor SDK Adapter Regimes* (arXiv:2605.12078). The paper's practical lesson is that ordinary traces, spans, tool-call logs, and message histories are not enough. A governed agent platform must be able to reconstruct, per decision/action:

- what input evidence existed;
- which policy/semantic authority applied;
- which operator/principal acted;
- what authorization envelope permitted or blocked the action;
- what reasoning evidence or safe rationale surrogate is available;
- what output action occurred;
- what post-condition state resulted.

The package turns that idea into an Ontogony engineering slice spanning:

- `allagma-dotnet` — run/operation/gate lifecycle fragments and decision-event assembly.
- `kanon-dotnet` — semantic authority, policy-basis binding, reconstructability classifier, governance score, missing-evidence diagnostics.
- `conexus-dotnet` — model call, route decision, provider/tool-call evidence fragments.
- `ontogony-frontend` — decision reconstruction workbench integrated into the Evidence Spine and run/trace views.
- `ontogony-ui` — reusable matrix, badges, diagnostics, and evidence-fragment components.
- `ontogony-platform` — protocol docs, local compose notes, cross-repo validation script.

## Non-negotiable design rule

Do **not** attempt to store or expose hidden chain-of-thought. The paper's “reasoning trace” property should be treated carefully. Ontogony should capture **safe reasoning surrogates**: explicit user-visible rationale, policy evaluation summaries, route-decision explanations, validation results, tool-selection grounds, and human/operator notes. Hidden model deliberation remains unavailable and should be classified as `O` unless a safe, explicit artifact exists.

## Intended result

After this package, a critical agent action should produce a reconstructability report like:

```text
Decision event: allagma.run.run-demo-001.operation.submit-human-gate
Inputs:                 F
Policy basis:           F
Operator identity:      F
Authorization envelope: F
Reasoning evidence:     P   safe surrogate present; no hidden CoT
Output action:          F
Post-condition state:   F
DES strict score:       78.6%
Ontogony governance:    PASS
Missing evidence:       none blocking
```

## Start here

1. Read `00_UNPACK_PROMPT.md`.
2. Follow `07_rollout/PHASED_EXECUTION_PLAN.md`.
3. Implement contracts from `03_contracts/` before frontend work.
4. Keep all new routes, DTOs, fixtures, and docs aligned with each repo's existing naming and route inventory conventions.
5. Do not remove or rewrite existing Evidence Spine behavior; extend it with reconstructability classification and diagnostics.
