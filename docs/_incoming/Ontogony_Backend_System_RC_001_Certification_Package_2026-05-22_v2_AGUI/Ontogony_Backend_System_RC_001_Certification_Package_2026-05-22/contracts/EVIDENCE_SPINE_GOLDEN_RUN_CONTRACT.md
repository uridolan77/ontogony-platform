# Evidence Spine Golden Run Contract

## Owner

`ontogony-platform` + `allagma-dotnet`

## Purpose

Define the cross-service evidence proof required for the evidence/audit/operator spine to score above 9.

## Golden run required nodes

```text
allagma_run
allagma_run_events
allagma_audit_bundle
allagma_interaction_events
kanon_planning_decision
kanon_action_policy_decision
kanon_human_gate_decision_optional
kanon_replay_bundle
conexus_model_call
conexus_route_decision
conexus_usage_cost_row
conexus_evidence_bundle
trace
correlation
```

## Required edges

```text
run_has_event
run_has_audit_bundle
run_references_kanon_decision
run_references_conexus_model_call
model_call_has_route_decision
model_call_has_usage_cost
kanon_decision_has_replay_bundle
trace_connects_services
correlation_connects_services
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

## Redaction checks

The exported evidence must not include:

```text
raw_prompt
raw_completion
provider_api_key
admin_key
service_token
unredacted_secret_reference
unapproved_pii_field
```

## Required artifacts

```text
golden-run-evidence-bundle.json
golden-run-evidence-graph.json
golden-run-redaction-report.json
negative-cases-summary.json
```
