# 11 — Platform Schemas and Drift Gates

**Repo:** `ontogony-platform`

## Goal

Make the Agent Interaction Spine a governed cross-repo contract, not an informal frontend convention.

## Files to add

```text
docs/operators/AGENT_INTERACTION_SPINE_CONTRACT.md
docs/system/agent-interaction-event.matrix.json
docs/schemas/ontogony-agent-interaction-event-v0.schema.json
docs/schemas/ontogony-agent-interaction-session-v0.schema.json
docs/evidence/PLAT_AGUI_000_EVIDENCE.md
scripts/validate-agent-interaction-spine.ps1
scripts/validate-agent-interaction-spine.sh
```

## Drift gates

Validation should check:

1. JSON schemas are valid.
2. All package fixtures validate against event schema.
3. Matrix references known service owners.
4. Required event families are present.
5. Required ID fields are documented.
6. Hidden reasoning / secrets redaction rules exist.
7. AG-UI adapter mapping covers lifecycle, messages, tools, state, interrupts, and custom events.

## Suggested matrix shape

```json
{
  "schema": "ontogony-agent-interaction-event-matrix-v0",
  "lastUpdated": "2026-05-22",
  "eventFamilies": [
    {
      "family": "RUN",
      "owner": "allagma",
      "requiredEvents": ["RUN_STARTED", "RUN_FINISHED", "RUN_ERROR"],
      "existingSources": ["/allagma/v0/runs/{runId}", "/allagma/v0/runs/{runId}/events"]
    }
  ]
}
```

## Acceptance evidence

`PLAT_AGUI_000_EVIDENCE.md` should include:

- schema validation command output
- fixture validation output
- list of event families
- known deferrals
- links to Allagma/Kanon/Conexus docs

## Versioning rule

Use additive `v0` changes until the first backend stream ships. Breaking changes require `v1` schema and dual-read frontend support.
