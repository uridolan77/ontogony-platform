# Data source taxonomy contract

## Allowed states

| State | Meaning | Can count as live readiness? |
|---|---|---:|
| `live` | Direct live backend/API response | yes |
| `live_with_fallback` | Live response with fixture/generated fallback used for missing fields or sections | only for live-covered fields |
| `fixture` | Static fixture/demo data | no |
| `generated` | Generated artifact, snapshot, route inventory, or build artifact | no, unless explicitly validated live |
| `imported` | User-imported bundle/JSONL/export | no |
| `mock` | Mocked local/test response | no |
| `unknown` | Source could not be determined | no |

## Display rules

- `live_with_fallback` must be a badge on affected cards, never a page title.
- `fixture`, `generated`, `imported`, and `mock` require visible non-live markers.
- Fixture/demo data must not be used in release-readiness summaries.
- Imported evidence can be useful, but it is not current live system state.

## Recommended labels

```text
Live data
Live data with fallback
Demo fixture — not live evidence
Generated artifact — not live validation
Imported bundle — historical evidence
Mock data — test mode
Data source unknown
```
