# KCP-002 — Domain-pack lifecycle and inventory polish

## Problem

The Domain Packs page currently risks conflating packs on disk, active ontology versions, selected pack, generated/test-looking versions, and lifecycle action availability.

## Scope

- Restructure inventory summary.
- Improve selected pack panel.
- Inline disabled lifecycle action reasons.
- Improve lifecycle timeline language.
- Clearly label simulation-only operations.
- Add tests.

## Implementation details

Inventory cards should distinguish, where data is available:

```text
Packs on disk
Persisted lifecycle versions
Active ontology versions
Deprecated versions
Generated/test-looking versions
```

Action availability should be rendered at the control:

```text
[Load disabled]
Reason: this version is already active.
```

Avoid a detached unavailable-actions block as the only explanation.

## Generated/test-looking names

If metadata exists, use it. If not, use defensive copy:

```text
Name pattern warning: this version name looks generated. Verify whether it belongs in the local operator dataset before promoting.
```

Do not claim it is a test artifact unless backend metadata says so.

## Acceptance

- Operator can understand why `Packs on disk: 2` and `Active ontology versions: 5` can coexist.
- Disabled actions are obvious and reason-bearing.
- Simulation outputs cannot be mistaken for promotion/load results.
