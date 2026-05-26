# Risk register

## Risk 1 — Weakening classifier to pass bad emitters

Do not fix classification failures by loosening Kanon rules unless the rule is demonstrably wrong for all services.

Default fix direction:

```text
Improve emitted evidence.
```

## Risk 2 — Frontend implemented before backend proof

Do not start a panel that renders fake or incomplete reconstructability results. Backend classification first.

## Risk 3 — Platform semantic pollution

Do not move classifier profiles, decision event kinds, run semantics, provider semantics, or policy meanings into Platform.

Platform may host protocol-neutral mechanics only.

## Risk 4 — Redaction leakage

Conexus model-call reconstructability is high risk. Decision events should use:

```text
promptHash
completionHash
modelAlias
routeDecisionId
modelCallId
usage/cost metadata
provider id
capability profile
```

not raw prompt/completion.

## Risk 5 — Overfitting to synthetic fixtures

Use at least one fixture derived from a real local fake-provider run, not only hand-built objects.

## Risk 6 — Ignoring WARN

WARN is acceptable for closure only if documented and not on a blocking property for the event profile.

## Risk 7 — Route/doc drift

Route inventory and OpenAPI drift silently erode agent trust. Regenerate inventories in the same PR as route additions.

## Risk 8 — Conexus streaming replay scope creep

Do not require durable streaming replay to close this package. Streaming lifecycle evidence with final hash/interruption metadata is enough for this path.

## Risk 9 — Allagma MGMT-008 side quest

Managed evaluation datasets matter, but do not let MGMT-008 polishing derail PR-001 unless stale dataset evidence directly affects reconstructability tests.
