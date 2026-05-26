# Regression test matrix

| Area | Scenario | Expected result |
|---|---|---|
| Health summary | liveness live, readiness ready, contract valid | Summary may say live/ready/usable. |
| Health summary | liveness live, readiness not_ready | Summary must say live but not ready, not healthy. |
| Health summary | liveness live, contract warning | Summary must say live with contract warning. |
| Health summary | liveness unknown | Label must say what is unknown and why. |
| Fixture fallback | live_with_fallback data source | Render badge, never page headline. |
| Fixture only | fixture data source | Render demo/fixture badge and exclude from readiness claims. |
| Evidence | no Kanon decision expected for direct Conexus call | Evidence state is not_applicable, not unresolved. |
| Evidence | missing expected route decision | Evidence state partial with reason code. |
| Topology | future edge exists in plan only | Render planned/future, not current/validated. |
| Topology | edge attempted and failed | Render degraded/missing with endpoint, last attempt, reason. |
| Settings | secret source missing | `Credential source: not set`, not `unknown`. |
| Allagma run list | task type absent | `Task type: unknown — not present in run metadata`, or hide if irrelevant. |
| Evaluation | provider missing | Do not present as strong quality evidence. |
| Developer details | API route references | Hidden in secondary/collapsible detail, not primary operator text. |
