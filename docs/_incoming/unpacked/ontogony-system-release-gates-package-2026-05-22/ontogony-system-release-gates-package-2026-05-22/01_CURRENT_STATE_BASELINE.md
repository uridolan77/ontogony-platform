# Current state baseline

## Runtime lock

Allagma currently owns `docs/system/ontogony-runtime.lock.json`. The lock records:

- system id;
- baseline id;
- expected refs;
- locked commits;
- package versions;
- API prefixes;
- ports;
- required scenarios;
- evidence paths.

Current known baseline before this package: `SYSTEM-ALPHA-006`.

## Existing package-mode proof

Allagma CI already contains an `allagma-package-mode` job that:

- checks out upstream Platform, Kanon, and Conexus into non-sibling pack-only paths;
- packs packages into a local feed;
- restores Allagma with:
  - `UseOntogonyPackages=true`
  - `UseKanonPackages=true`
  - `UseConexusPackages=true`
- builds and tests Allagma without sibling project references.

PLATFORM-REL-001 should elevate this from CI implementation detail into an explicit release-train contract.

## Existing system cohesion

Allagma owns the system compatibility matrix and required smoke scenarios. Current runtime lock required scenarios include:

- correlation chain;
- completed run;
- idempotent run start;
- human gate waiting;
- human gate approved;
- human gate denied;
- Kanon → Conexus assistance;
- Conexus fallback;
- restart survival.

SYSTEM-ALPHA-008 extends this into scheduled/manual execution and adds streaming + capacity baseline into the scheduled suite.

## Existing Conexus capacity baseline

Conexus NEXT-009 added an opt-in capacity baseline gated by:

```text
CONEXUS_RUN_CAPACITY_BASELINE=true
```

It writes JSON/Markdown reports and can optionally run Postgres scenarios. SYSTEM-ALPHA-008 should call this as a sub-gate instead of reinventing capacity tests.

## Existing Kanon contract gates

Kanon already has route inventory, OpenAPI baseline, compatibility manifest, v0 freeze, client coverage, evidence spine handoff, and Postgres semantic smoke. SYSTEM-ALPHA-007 should run or collect those gates in release mode.

## What is missing

1. No one-command locked-runtime release command.
2. No single release evidence bundle aggregating all four repos.
3. No first-class release train contract for package-mode.
4. No scheduled/manual full-system cohesion workflow that includes all required scenarios plus streaming and capacity.
5. No standardized JSON schema for release evidence bundles.
6. No PASS/FAIL policy for drift vs release evidence.
