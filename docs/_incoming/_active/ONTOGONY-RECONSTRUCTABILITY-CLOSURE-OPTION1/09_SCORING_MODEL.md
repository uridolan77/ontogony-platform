# Backend scoring model

This package uses the following practical scoring model.

## 8.x

A repo is strong alpha:

```text
major features implemented
tests pass
boundaries documented
local evidence exists
known limitations honest
```

## 9.0+

A repo is system-closed:

```text
core feature is proven against sibling repos
contract drift is guarded
current-state docs match code
operator evidence exists
high-risk paths have negative tests
```

## 9.5+

A repo is release-candidate mature:

```text
production-like deployment assumptions tested
multi-replica/persistence concerns covered
SLO evidence live
security/IAM reviewed
runbooks complete
```

This package targets 9.0+, not 9.5+.

## Repo-specific 9+ criteria

### Allagma

```text
Kanon classifies Allagma high/critical decision events with no FAIL.
Durable execution evidence survives restart/recovery.
Run/trace/model/decision IDs traverse cleanly.
```

### Kanon

```text
Classifies external Allagma and Conexus events.
Profiles are event-kind aware.
Trace-level reconstructability is useful and actionable.
```

### Conexus

```text
Emits reconstructable model access decisions.
No raw prompt/secret leakage.
Routing/fallback/quota/cache/model-call evidence classifies.
```

### Platform

```text
Consumers run conformance kits for shared mechanics.
Public API/package adoption is tracked.
No product semantics leak into shared packages.
```
