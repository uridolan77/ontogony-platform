# PR-PLAT-NP-007 — Secret reference parser

## Goal

Add a small cloud-neutral parser for secret references, useful to Conexus and future services.

## Proposed API

- `SecretValueReferenceParser.TryParse(string value, out SecretValueReference reference, out string? error)`
- Accepts `scheme:locator`.
- Rejects blank scheme/locator.
- Does not validate scheme-specific semantics.
- Does not add cloud SDKs.

## Acceptance

- Tests for `env:NAME`, `vault:path`, invalid/no-colon, blank scheme, blank locator.
- Docs in `Ontogony.Secrets/README.md`.
- Public API snapshot updated.

## Non-goals

- No AWS/Azure/GCP vault resolver.
- No Conexus provider config semantics.
