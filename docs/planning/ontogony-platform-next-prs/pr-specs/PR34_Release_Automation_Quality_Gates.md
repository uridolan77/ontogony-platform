# PR34 — Release Automation and Quality Gates

## Goal

Make package release safe, repeatable, and auditable.

## Scope

Add workflows:

- `ci.yml` remains restore/build/test/pack.
- `release-packages.yml` runs on tag or manual dispatch.
- package publish dry-run.
- internal feed publish path.
- artifact upload.
- package manifest generation.

Add package metadata maturity:

- README packaged with nupkg.
- license metadata or license file.
- repository URL/type.
- symbols/source package option.
- SourceLink if desired.

## Package manifest

Generate:

```json
{
  "version": "0.3.0",
  "commit": "...",
  "packages": [
    { "id": "Ontogony.Contracts", "version": "0.3.0" }
  ]
}
```

## Quality gates

- No package produced = fail.
- Version missing = fail.
- Changelog missing for non-doc changes = fail if script feasible.
- Public API change without migration note = warn/fail in later PR.

## Acceptance

A maintainer can publish internally from a tagged commit and retrieve a manifest proving what shipped.
