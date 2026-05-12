# PR31 — Schema Governance and Compatibility

## Goal

Prevent accidental breaking changes to envelopes, headers, and public package APIs.

## Scope

Add:

- schema version policy
- JSON schema fixture tests
- CloudEvents round-trip golden vectors
- public API snapshot tests or generated API manifest
- header compatibility matrix
- breaking-change checklist

## Files

```text
schemas/fixtures/valid/*.json
schemas/fixtures/invalid/*.json
docs/contracts/compatibility-policy.md
docs/contracts/header-compatibility-matrix.md
tools/ or scripts/check-public-api.ps1
```

## Tests

- Valid fixture envelopes pass schema and validator.
- Invalid fixtures fail with expected reason codes.
- CloudEvents fixture round-trips preserve required fields.
- Header constants remain stable unless expected snapshot is updated.

## Must not do

- Do not validate product payloads.
- Do not encode Athanor/Agentor payload semantics.

## Acceptance

A PR that changes wire contracts must update fixtures, schema version policy, changelog, and migration notes.
