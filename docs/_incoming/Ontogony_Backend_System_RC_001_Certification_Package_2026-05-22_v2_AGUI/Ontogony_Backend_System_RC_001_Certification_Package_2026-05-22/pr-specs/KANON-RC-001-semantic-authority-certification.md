# KANON-RC-001 — Semantic Authority Certification

## Owner

`kanon-dotnet`

## Problem

Kanon is strong and already near above-9, but its RC1 status needs hard certification through manifest gates, negative semantic authority tests, and Postgres smoke as a release gate.

## Goal

Certify Kanon as the semantic authority for the current alpha-RC.

## Required files

```text
docs/system/KANON_SEMANTIC_AUTHORITY_CERTIFICATION_MATRIX.md
docs/evidence/KANON_RC_001_SEMANTIC_AUTHORITY_CERTIFICATION_EVIDENCE.md
```

Optional machine-readable version:

```text
docs/system/kanon-semantic-authority-certification.matrix.json
```

## Required matrix rows

```text
v0_route_inventory_matches_runtime
openapi_baseline_matches_runtime
client_coverage_matches_manifest
server_only_routes_are_not_in_client
v1_routes_absent
invalid_ontology_version
unapproved_source_binding_rejected_or_not_used
contradiction_unresolved_goes_to_review
policy_denied
human_gate_required
human_gate_approved
human_gate_denied
assistance_draft_created_non_authoritative
assistance_draft_accepted
assistance_draft_rejected
domain_pack_promotion_blocked
domain_pack_rollback_plan_generated
replay_manifest_export_hash_valid
replay_manifest_export_hmac_valid_when_configured
postgres_semantic_smoke
```

Each row must include:

```text
route_or_component
test_filter
expected_decision_or_status
evidence_artifact
```

## Required commands

```powershell
dotnet test Kanon.sln -c Release --filter "FullyQualifiedName~KanonCompatibilityManifestTests"
dotnet test Kanon.sln -c Release --filter "FullyQualifiedName~OntologyV0RouteInventoryTests"
dotnet test Kanon.sln -c Release --filter "FullyQualifiedName~OpenApiBaselineTests"
dotnet test Kanon.sln -c Release --filter "FullyQualifiedName~KanonV1GraduationGuardTests"
dotnet test Kanon.sln -c Release --filter "FullyQualifiedName~PostgresSemanticSmokeTests"
```

## Acceptance criteria

- `KANON_COMPATIBILITY_MANIFEST.json` remains current.
- `/ontology/v0` remains the only ontology route family.
- `/ontology/v1` is absent.
- Client/server-only route counts match manifest.
- Negative semantic authority cases are covered by tests.
- Conexus assistance remains non-authoritative and review-gated.
- Postgres semantic smoke is part of RC gate.

## Non-goals

- No `/ontology/v1` promotion.
- No model routing in Kanon.
- No orchestration semantics in Kanon.
