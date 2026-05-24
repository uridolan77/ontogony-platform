# Migration map

| Existing phrase/pattern | Target taxonomy treatment |
|---|---|
| `Live with fixture fallback` headline | Data-source badge: `live_with_fallback`; page title remains product/workflow name. |
| `All services are healthy` | Derived summary only when connectivity live, readiness ready, contract valid, usability usable for all required services. |
| `health payload format warning` | Contract health warning with missing/mismatched field list. |
| `Gateway health` on Kanon page | `Kanon API health` or `Conexus gateway health`, depending on actual source. |
| `unknown` | `{subject}: unknown — {reason}`. |
| `Failed runs (sample)` | `Failed runs in current list` or `Failed runs, last 24h`. |
| `Backend-waiting list APIs` | Remove from primary UI; move to developer details if still useful. |
| `Fixture replay` | `Demo fixture — not live evidence`. |
| `Provider unknown` | `Provider: unknown — model-call metadata did not include provider`. |
| `No kill switch` | `Kill switch: not configured — local alpha warning`. |
| planned topology link | `planned` edge with lower visual weight and no readiness credit. |
| direct Conexus missing Kanon decision | `not_applicable`, not `unresolved`. |
