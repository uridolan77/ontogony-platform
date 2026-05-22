# SYSTEM-RC-001E — Evidence Spine Golden Run

## Owner

`ontogony-platform` + `allagma-dotnet`

## Problem

Evidence/audit/operator spine is deep, but above-9 certification requires one deterministic golden journey that proves a run can be traced across Allagma, Kanon, Conexus, usage/cost, replay, and redaction boundaries.

## Goal

Create and validate a golden evidence run.

## Required golden journey

```text
Start Allagma run
→ Kanon semantic plan decision
→ Kanon topology/action policy decision
→ optional human gate event
→ Conexus model call
→ Conexus route decision
→ Conexus usage/cost row
→ Allagma run audit bundle
→ Allagma interaction event export
→ Kanon replay bundle
→ cross-service evidence graph
→ redaction report
```

## Required artifacts

```text
artifacts/evidence-spine/<timestamp>/golden-run-evidence-bundle.json
artifacts/evidence-spine/<timestamp>/golden-run-evidence-graph.json
artifacts/evidence-spine/<timestamp>/golden-run-redaction-report.json
docs/evidence/SYSTEM_RC_001E_EVIDENCE_SPINE_GOLDEN_RUN.md
```

## Required negative cases

```text
missing_kanon_decision
missing_conexus_model_call
missing_route_decision
unauthorized_project_key
admin_key_without_read_scope
streaming_usage_missing
human_gate_denied
```

Each case should produce a deterministic completeness/missing-reason result, not an unhandled exception.

## Redaction requirements

The redaction report must verify absence of:

```text
raw_prompt
raw_completion
provider_api_key
admin_key
service_token
unredacted_secret_reference
unapproved_pii_field
```

## Acceptance criteria

- Golden evidence graph resolves all expected nodes/edges.
- Negative cases return expected missing/completeness classification.
- Redaction report passes.
- Artifacts are linked from release evidence index.
- No raw prompt/completion/secrets are included in default exported evidence.

## Non-goals

- No new operator UI feature required.
- No production retention policy implementation.
- No real external tool execution.
