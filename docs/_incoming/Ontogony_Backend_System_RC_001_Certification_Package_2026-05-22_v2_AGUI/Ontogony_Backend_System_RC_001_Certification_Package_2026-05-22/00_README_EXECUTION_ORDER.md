# Ontogony Backend SYSTEM-RC-001 Certification Package

**Package:** `Ontogony_Backend_System_RC_001_Certification_Package_2026-05-22`  
**Purpose:** Move the four backend repos from strong alpha (`8.x`) to certified alpha-RC (`9+`) through lock promotion, machine validation, live evidence, observability PASS, and stale-doc cleanup.

## Repos in scope

| Repo | Role |
|---|---|
| `uridolan77/ontogony-platform` | Shared mechanics, package substrate, protocol registry |
| `uridolan77/conexus-dotnet` | Model gateway, provider routing, usage/cost, model-call evidence |
| `uridolan77/kanon-dotnet` | Semantic authority, ontology/facts/policy/human gates/provenance |
| `uridolan77/allagma-dotnet` | Governed runtime, orchestration, runtime lock, system cohesion |

## Guiding principle

This package is **not a feature expansion package**. It is a **certification package**.

The goal is to prove that the current backend system is:

1. pinned;
2. buildable in sibling-source and package modes;
3. cross-repo integrated through typed clients;
4. restart-safe with durable paths;
5. observable with real PASS evidence;
6. audit/evidence-resolvable end-to-end;
7. free of stale release-blocking docs;
8. explicit about non-goals: no production readiness claim, no enterprise IAM claim, no real external tool execution.

## Execution order

Run the PRs in this order:

1. `SYSTEM-RC-001A-runtime-lock-promotion`
2. `SYSTEM-RC-001B-full-cohesion-evidence`
3. `SYSTEM-RC-001C-package-mode-certification`
4. `SYSTEM-RC-001D-observability-pass`
5. `SYSTEM-RC-001E-evidence-spine-golden-run`
6. `CONEXUS-RC-001-gateway-certification-matrix`
7. `KANON-RC-001-semantic-authority-certification`
8. `PLATFORM-RC-001-substrate-contract-freeze`

## Hard rule

Do not merge a later PR if an earlier RC gate is red, unless the later PR is explicitly a fix for the failing gate.

## Desired score movement

| Area | Before | Target after package |
|---|---:|---:|
| Cross-repo integration | 8.0 | 9.2 |
| Runtime orchestration | 8.5 | 9.2 |
| Semantic authority | 8.8 | 9.3 |
| Model gateway | 8.6 | 9.2 |
| Shared platform mechanics | 8.2 | 9.1 |
| Evidence / audit / operator spine | 8.0 | 9.3 |
| Observability | 7.0 | 9.1 |

## Completion definition

The package is complete when:

```text
pwsh ./scripts/validate-system-tight-rc-prep.ps1
pwsh ./scripts/validate-system-tight-rc-readiness.ps1
pwsh ./scripts/validate-system-tight-rc-evidence.ps1
```

all pass, latest selected heads are pinned, and `RELEASE_EVIDENCE_INDEX.md` points to current PASS artifacts.


---

## AG-UI / Agent Interaction Spine addendum (v2)

This package now includes an explicit backend certification track for the AG-UI work we started to integrate. In this package, **AG-UI means the backend event/export/streaming contract that makes a governed run renderable and replayable by an agent-interaction UI**, not a frontend-only implementation task.

New required slice:

```text
SYSTEM-RC-001F — Agent Interaction / AG-UI spine certification
```

Place it after the evidence-spine golden-run slice and before final release closeout. It certifies:

- Platform owns the canonical `ontogony-agent-interaction-event-v0` schema and validator.
- Allagma exports run interaction events as JSONL and streams them over SSE.
- Conexus projects model-call lifecycle into agent interaction events with evidence-bundle links.
- Kanon human-gate / review-queue semantics are representable as interrupt / resume / review events.
- The generated event stream is deterministic, redacted, resume-safe, and sufficient for operator replay.

Hard boundary: this is **backend contract certification**, not a requirement to finish all frontend AG-UI components in this sprint.
