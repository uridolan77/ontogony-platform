# CONEXUS-RC-001 — Gateway Certification Matrix

## Owner

`conexus-dotnet`

## Problem

Conexus is a strong alpha gateway, but to score above 9 it needs a single certification matrix tying gateway behavior to tests, scripts, and evidence.

## Goal

Create and validate a gateway certification matrix covering routing, fallback, quotas, idempotency, streaming, model-call evidence, usage/cost drilldown, and auth isolation.

## Required files

```text
docs/system/CONEXUS_GATEWAY_CERTIFICATION_MATRIX.md
docs/evidence/CONEXUS_RC_001_GATEWAY_CERTIFICATION_EVIDENCE.md
```

Optional machine-readable version:

```text
docs/system/conexus-gateway-certification.matrix.json
```

## Required matrix rows

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

Each row must include:

```text
route
test_filter
expected_status
expected_error_code_or_missing_reason
evidence_doc_or_artifact
```

## Required commands

Use exact test names if available; otherwise add them.

Suggested filters:

```powershell
dotnet test Conexus.sln -c Release --filter "FullyQualifiedName~ModelCallEvidenceBundle"
dotnet test Conexus.sln -c Release --filter "FullyQualifiedName~UsageCostDrilldown"
dotnet test Conexus.sln -c Release --filter "FullyQualifiedName~RouteDecision"
dotnet test Conexus.sln -c Release --filter "FullyQualifiedName~Streaming"
dotnet test Conexus.sln -c Release --filter "FullyQualifiedName~Quota"
dotnet test Conexus.sln -c Release --filter "FullyQualifiedName~Idempotency"
dotnet test Conexus.sln -c Release --filter "FullyQualifiedName~Fallback"
```

## Acceptance criteria

- Matrix exists and every row has a test or explicit deferred status.
- No certified row is docs-only.
- Model-call evidence bundle has redaction tests.
- Usage-cost drilldown links to evidence bundle and route decision.
- Project key cannot access another project’s model-call evidence.
- Admin scoped keys enforce read/write/manage scopes.
- Streaming idempotency boundary is tested.

## Non-goals

- No need to finish every provider’s full parity.
- No real tool execution.
- No enterprise IAM runtime implementation.
