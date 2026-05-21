# Operator failure taxonomy adapter (SYS-TIGHT-006)

## Summary

Adds operator-facing failure taxonomy (`OperatorFailureTaxonomyKind`, `OperatorFailureView`, `OperatorFailureTaxonomyAdapter`) in `Ontogony.Errors`, a cross-repo contract index, and a representative mapping matrix. Frontend and Allagma consume the taxonomy at the adapter layer only.

## Consumers

- `ontogony-frontend` — `operatorFailureTaxonomy.ts`, `OperatorFailureBanner`, query/run audit integration.
- `allagma-dotnet` — additive `operatorFailureTaxonomy` fields on run failure payloads.

## Breaking changes

None. Additive contract only. Kanon, Conexus, and Allagma public HTTP error shapes are unchanged.

## Repos to update together

- `ontogony-platform` (this change)
- `allagma-dotnet` (run failure payload + tests)
- `ontogony-frontend` (banner + adapter tests)

Package consumers on pinned `Ontogony.Errors` must bump after platform release for C# adapter types.
