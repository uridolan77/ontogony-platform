# Production IAM gate

## Current state

Aisthesis supports auth modes:

```text
off
shared-token
producer-token
```

Producer-token mode with body-level producer enforcement is alpha-hardening, not a full production IAM model.

## Production IAM questions

1. Who can write evidence?
2. Who can read evidence?
3. Who can export bundles?
4. Who can run evaluations?
5. How are producer tokens rotated?
6. How are failed auth attempts audited?
7. How are read permissions separated from write permissions?
8. How are service-to-service identities represented?
9. How are secrets stored outside local env vars?
10. How are tenant/customer boundaries represented, if any?

## Minimum RC gate

For RC-readiness, document:

```yaml
iamModeForRC:
tokenRotation:
readWriteSeparation:
secretStorage:
auditLog:
knownGaps:
productionBlocker: true|false
```

Do not implement complex IAM unless explicitly scoped.
