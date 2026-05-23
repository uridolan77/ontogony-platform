# 01 — Current Diagnosis

## Observed symptoms

The current local operator console shows:

```text
Baseline: SYSTEM-ALPHA-006
Runtime lock: locked
Evidence index reconciliation: ready
Revalidation queue: clear
Postgres: enabled
Allagma: live / ready
Kanon: live / ready
Conexus: live / strict readiness not ready
All services: health payload format warning
System compatibility: unknown
No compatibility summary artifact on disk
No version metadata from health endpoints
```

The console also surfaces wording like:

```text
Live with fixture fallback
Service versions appear aligned
All services are healthy
Conexus health: healthy
Conexus readiness strict: not ready
```

These may all be individually explainable, but together they are operator-hostile.

## Core diagnosis

The problem is not basic connectivity. The system is running.

The problem is **truth presentation**:

- liveness, readiness, health schema validity, compatibility, and release-readiness are conflated;
- fixture/demo/generated states appear beside live states without hierarchy;
- missing version metadata prevents actual compatibility verification;
- Conexus is usable for fake-provider calls but not strict-ready, and the console does not explain exactly why;
- readiness artifacts appear generated/mechanical but are presented too optimistically;
- "unknown" appears in important places without severity or remediation.

## What SYSTEM-TRUTH-001 must prove

After this workstream, an operator should be able to answer:

1. Is each service reachable?
2. Is each service ready?
3. Is each service returning the expected health/readiness contract?
4. Which exact readiness check is failing?
5. Which services are version-aligned?
6. Is the compatibility summary live/generated/stale/missing?
7. Which console values are live, fixture, generated, imported, or unknown?
8. Is release-readiness assessed from live evidence, generated route inventory, fixture data, or not assessed?
9. Can I run one smoke command and get a machine-readable truth summary?
