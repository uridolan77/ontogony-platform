# Production IAM and edge authorization gate

## Current acceptable alpha state

Producer-token mode with body-level producer enforcement is good alpha hardening.

## RC concern

Direct edge writes can be more dangerous than envelope writes because edges define cross-system causality. Edge spoofing can make an unreconstructable trace appear reconstructable.

## Minimum edge-auth policy

Choose one:

### Option A — Batch-only producer edge writes

Direct edge route requires wildcard/system token. Producer-specific tokens must submit edges through a batch with `producerSystem`.

### Option B — Add producer identity to direct edge contract

Direct edge requests include `producerSystem`, and authorization validates token scope.

### Option C — Relation-scoped ACL

Only the owning producer can emit relations under its namespace:

```text
allagma.* -> allagma token
kanon.* -> kanon token
conexus.* -> conexus token
metabole.* -> metabole token
```

Wildcard/system token may repair/admin edges if audited.

## Production IAM promotion questions

- Are tokens hashed at rest?
- Can tokens rotate without downtime?
- Are read/write/evaluate scopes separate?
- Are bundle export permissions separate?
- Are failed auth attempts audited?
- Are tenant/customer boundaries represented?
- Is secret storage outside local env vars?

## Closeout requirement

Classify IAM as:

```yaml
implemented: true|false
rcScopeAccepted: true|false
productionBlocker: true|false
reason:
owner:
```
