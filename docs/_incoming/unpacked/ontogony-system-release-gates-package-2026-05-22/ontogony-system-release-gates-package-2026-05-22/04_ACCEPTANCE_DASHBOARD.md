# Acceptance dashboard

## SYSTEM-ALPHA-007

| Gate | Required | Evidence |
|---|---:|---|
| Runtime lock parsed | yes | `runtime-lock.snapshot.json` |
| Four repo commits checked out | yes | `repo-commits.json` |
| Platform build/test | yes | `repo-gates/ontogony-platform/summary.json` |
| Kanon build/test contract gates | yes | `repo-gates/kanon-dotnet/summary.json` |
| Conexus build/test default filter | yes | `repo-gates/conexus-dotnet/summary.json` |
| Allagma build/test/conformance | yes | `repo-gates/allagma-dotnet/summary.json` |
| Package-mode release train | yes | `package-mode/package-mode-release-summary.json` |
| System cohesion suite | yes | `system-cohesion/system-cohesion-scheduled-summary.json` |
| Release evidence schema validation | yes | `validation/summary.json` |
| Release verdict | yes | `release-evidence-bundle.json.verdict` |

## SYSTEM-ALPHA-008

| Scenario | Required | Notes |
|---|---:|---|
| Completed governed run | yes | Allagma → Kanon → Conexus |
| Idempotent run start | yes | Existing lock scenario |
| Human gate waiting | yes | Kanon policy path |
| Human gate approved | yes | resume/approve |
| Human gate denied | yes | deny terminal path |
| Kanon Conexus assistance | yes | draft-only + decision record |
| Conexus fallback | yes | fake retry/fallback path |
| Restart survival | yes | Postgres-backed restart |
| Streaming smoke | yes | metadata-only chunk lifecycle acceptable |
| Capacity baseline | yes | Conexus NEXT-009 local alpha thresholds |

## PLATFORM-REL-001

| Gate | Required | Notes |
|---|---:|---|
| Pack Ontogony packages | yes | Version from runtime lock |
| Pack Kanon packages | yes | `Kanon.Client`, `Kanon.Contracts` |
| Pack Conexus packages | yes | `Conexus.Client`, `Conexus.Contracts` |
| Restore Allagma with package flags | yes | no sibling refs |
| Build Allagma package-mode | yes | release configuration |
| Test Allagma package-mode | yes | exclude only documented opt-in categories |
| Version alignment with runtime lock | yes | hard fail on mismatch |
| Evidence summary | yes | JSON + Markdown |

## Failure classes

| Class | Meaning |
|---|---|
| `lock.invalid` | Runtime lock missing or malformed |
| `checkout.failed` | Pinned repo commit unavailable |
| `repo_gate.failed` | Repo-local build/test gate failed |
| `contract_gate.failed` | Route/OpenAPI/manifest/client contract gate failed |
| `package_mode.failed` | Package restore/build/test failed |
| `cohesion.failed` | Runtime scenario failed |
| `capacity.failed` | Conexus capacity threshold failed |
| `evidence.invalid` | Evidence bundle schema or policy invalid |
| `drift.detected` | Expected-ref run differs from lock; not a release failure unless in locked mode |
