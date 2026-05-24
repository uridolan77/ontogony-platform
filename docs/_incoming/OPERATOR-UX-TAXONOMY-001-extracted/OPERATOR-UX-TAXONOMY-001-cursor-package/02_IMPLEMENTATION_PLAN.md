# Implementation plan

## Phase 0 — Audit current status vocabulary

Search both `ontogony-ui` and `ontogony-frontend` for:

```text
healthy
health
ready
not ready
unknown
live
fixture
fallback
generated
sample
demo
authoritative
advisory
partial
unresolved
warning
blocked
Gateway health
Live with fixture fallback
Failed runs (sample)
health payload format warning
No kill switch
```

Produce a short audit note before coding:

- component/file path;
- existing vocabulary;
- target taxonomy dimension;
- migration action.

## Phase 1 — Add shared taxonomy in `@ontogony/ui`

Add a public subpath such as one of:

- `@ontogony/ui/operator-status`
- `@ontogony/ui/status-taxonomy`
- existing shared status family if already present

Export:

- enums/string unions for each dimension;
- label and description helpers;
- severity mapping;
- badge/card view-model builders;
- no-bare-unknown guard helpers;
- optional React primitives if consistent with repo style.

## Phase 2 — Map frontend adapters into taxonomy

Create product-level adapter functions in `ontogony-frontend`, for example:

- `mapHealthToOperatorStatus(...)`
- `mapReadinessToOperatorStatus(...)`
- `mapEvidenceCompleteness(...)`
- `mapDataSource(...)`
- `mapAuthority(...)`
- `mapTopologyEdgeState(...)`

The adapters should preserve detailed reasons; do not reduce backend failures to a vague color.

## Phase 3 — Migrate high-impact pages

Prioritize pages that currently create operator confusion:

1. Home / service health cards
2. System compatibility and topology readiness
3. Evidence Spine resolution summary
4. Agent Interaction mode selector/session state
5. Kanon overview/source bindings/domain packs
6. Allagma run list/runtime posture
7. Conexus model/provider posture
8. Settings credential source labels
9. Evaluation evidence quality

## Phase 4 — Remove misleading copy and bad patterns

Eliminate or quarantine:

- page-headline `Live with fixture fallback`;
- bare `unknown`;
- `healthy` when readiness/contract says warning/not-ready;
- backend route/debug implementation notes in primary cards;
- `sample` without explaining sample what;
- future/planned links presented as current.

## Phase 5 — Tests and validation scripts

Add local tests in the repos and provide scripts that scan for forbidden strings and missing labels.

## Phase 6 — Final review

Run tests, lint, build, and a manual console sweep. Verify that major pages now speak one taxonomy.
