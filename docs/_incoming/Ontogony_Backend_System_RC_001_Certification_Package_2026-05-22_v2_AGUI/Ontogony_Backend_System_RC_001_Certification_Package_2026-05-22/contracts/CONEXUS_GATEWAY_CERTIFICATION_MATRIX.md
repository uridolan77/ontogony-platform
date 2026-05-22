# Conexus Gateway Certification Matrix Contract

## Owner

`conexus-dotnet`

## Purpose

Define the gateway behaviors required for Conexus to score above 9 as model gateway.

## Matrix schema

Each row must include:

```text
id
surface
route_or_component
preconditions
expected_result
expected_status_or_error
redaction_requirement
test_filter
evidence_artifact
release_gate_required
```

## Required rows

```text
alias_resolution_success
unknown_alias_rejected
provider_disabled
primary_fail_fallback_success
all_providers_fail
quota_allow
quota_deny
idempotent_non_streaming_replay
idempotency_conflict
streaming_success
streaming_rejects_idempotency_key
streaming_terminal_usage_present
streaming_usage_missing_classified
model_call_evidence_bundle_redacted
usage_cost_drilldown_linked
route_decision_linked
admin_scoped_key_read
admin_scoped_key_write_rejected_on_read_route
project_scoped_evidence_isolation
```

## Certified provider policy

The RC may certify a narrow provider set.

Allowed status values:

```text
certified
supported_not_rc_certified
experimental
disabled_by_default
deferred
```

A provider must not be implied as RC-certified unless matrix rows prove it.
