# 01 — Executive Plan

## Why now

The fake-provider Docker-local system passed manual QA. The next risk is whether the same operator paths work when Conexus routes a controlled request to a real provider.

## Validation target

A successful phase means:

```text
One local operator can run one small real-provider smoke,
observe it through Conexus / Allagma / Kanon / frontend surfaces,
record sanitized evidence,
then return safely to fake-provider mode.
```

## Non-goals

- production readiness
- cloud deployment
- real user traffic
- load/performance tests
- provider benchmark suite
- secret management platform
- external sandbox/tool execution
