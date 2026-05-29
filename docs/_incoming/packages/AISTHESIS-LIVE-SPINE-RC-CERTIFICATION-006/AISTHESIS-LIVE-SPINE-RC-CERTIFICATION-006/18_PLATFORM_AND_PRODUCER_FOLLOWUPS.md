# Platform and producer follow-ups

## Platform

`ontogony-platform` should own the system-level lock review:

```text
docs/evidence/SYSTEM_RC_AISTHESIS_LOCK_REVIEW_006.md
docs/system/ONTOGONY_FIVE_SERVICE_EVIDENCE_CERTIFICATION_MATRIX.md
```

The platform doc should state whether Aisthesis can be used as the RC evidence certification point for the whole system.

## Producers

Producer repos should not be modified unless Aisthesis v2 certification reveals missing evidence.

### Allagma must emit

- run lifecycle;
- semantic plan request/use;
- decision linkage;
- model call linkage;
- human gate linkage;
- tool intent/execution;
- side-effect/output artifact when applicable.

### Kanon must emit

- semantic plan;
- decision;
- policy evaluation;
- canonical facts/source binding where applicable;
- human gate opened/resolved;
- replay bundle where applicable.

### Conexus must emit

- model call requested/completed/failed;
- route decision;
- provider attempt;
- provider fallback/error;
- usage/cost where available;
- streaming summary where applicable.

### Metabole must emit

- pipeline run;
- schema extract;
- data profile;
- mapping candidate;
- review outcome;
- artifact/package;
- transformation plan/output where applicable.
