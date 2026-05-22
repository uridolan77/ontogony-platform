# Semantic Authority Certification Matrix Contract

## Owner

`kanon-dotnet`

## Purpose

Define the semantic authority behaviors required for Kanon to score above 9.

## Matrix schema

Each row must include:

```text
id
semantic_surface
route_or_component
authority_rule
expected_decision_or_status
test_filter
evidence_artifact
release_gate_required
```

## Required rows

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

## Hard rule

Kanon may call Conexus only for non-authoritative assistance. Conexus output must not become canonical truth without Kanon review/decision semantics.
