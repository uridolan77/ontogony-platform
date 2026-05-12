# PR35 — Documentation-First Developer Experience

## Goal

Turn the repo from “lots of good docs” into a coherent platform manual.

## Scope

Create a docs map with explicit paths:

```text
docs/start-here.md
docs/architecture/
docs/packages/
docs/hosting/
docs/security/
docs/observability/
docs/persistence/
docs/protocol-ingress/
docs/testing/
docs/adoption/
docs/migrations/
```

Add:

- package quick-starts
- package dependency graph
- “which package do I need?” guide
- service adoption playbooks
- operational checklists
- decision records index
- examples index

## Must not do

- Do not over-market.
- Do not claim production completeness where only reference implementations exist.
- Do not bury warnings about in-memory/reference packages.

## Acceptance

A new developer can answer in under 15 minutes:

1. What is this platform?
2. Which packages are stable?
3. How do I wire a new service?
4. How do I publish packages?
5. How do I validate adoption?
6. What must not go into the platform?
