# PR-PLAT-NP-002 — Real Conexus package-mode compatibility

## Goal

Prove the real `conexus-dotnet` repo can consume Ontogony packages rather than sibling project references.

## Preferred implementation

Implement mostly in Conexus CI. Platform may add docs and a contract.

## Platform scope

- Add a compatibility contract doc defining required package feed/auth/version inputs.
- Keep the existing Conexus package smoke.
- Do not import Conexus product code into Ontogony.

## Acceptance

- A Conexus workflow can restore/build/test using packed or published Ontogony packages.
- The workflow fails if it accidentally uses sibling project references.
- Package version is explicit.
- Result is documented from both repos.

## Non-goals

- No Conexus route/provider/pricing semantics in Ontogony.
