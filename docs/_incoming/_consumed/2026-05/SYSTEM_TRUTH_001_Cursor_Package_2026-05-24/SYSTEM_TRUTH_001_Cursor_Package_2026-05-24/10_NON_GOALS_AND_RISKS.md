# 10 — Non-goals and Risks

## Non-goals

This workstream does not:

- make the system production-ready;
- enable real external tool execution;
- complete the Evidence Spine across all node types;
- redesign Kanon source bindings;
- implement new provider integrations;
- implement Agent Interaction live timeline;
- fix every evaluation-dashboard limitation;
- build a full domain switcher.

## Risks

### Risk: health contracts become too heavy

Mitigation: keep `/health` lightweight. Put dependency checks in `/ready`.

### Risk: Conexus readiness becomes blocked by optional real providers

Mitigation: check severity. Optional OpenAI credentials should be warning unless aliases require OpenAI.

### Risk: frontend still has to support old payloads

Mitigation: support legacy parser as degraded/contract-warning fallback for one transition window.

### Risk: generated compatibility artifact becomes stale

Mitigation: include `generatedAtUtc` and display staleness.

### Risk: too much cross-repo churn

Mitigation: implement service-local DTOs first if platform extraction is too disruptive. Keep schemas identical.

### Risk: "not ready" alarms operators during local fake mode

Mitigation: separate readiness from operator usability:
- service may be `degraded`;
- local fake-path usability may be `usable`.
