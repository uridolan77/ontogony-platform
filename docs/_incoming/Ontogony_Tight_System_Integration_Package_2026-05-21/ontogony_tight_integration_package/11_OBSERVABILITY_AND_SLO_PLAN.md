# 11 — Observability and SLO plan

## Goal

Move from local evidence artifacts to an operator-ready observability posture while keeping alpha/prod boundaries clear.

## Cross-service observability model

| Signal | Allagma | Kanon | Conexus | Platform |
|---|---|---|---|---|
| Trace/correlation | run lifecycle | decisions/provenance | model calls/routes | propagation primitives |
| Metrics | run duration/state/failure | decision/provenance/gate metrics | provider/model/usage/quota metrics | shared instrument conventions |
| Logs | orchestration stages | semantic decisions | gateway/provider attempts | structured log conventions |
| Evidence | run audit bundle | decision/provenance/replay | model-call evidence bundle | evidence registry/spine |

## Minimum dashboard v1

### Runtime health

- `/health` and `/ready` for Allagma, Kanon, Conexus.
- Current lock baseline and commit pins.
- Current package versions.
- Service startup validation status.

### Run execution

- runs started/completed/failed/cancelled/retried.
- human gate waiting count.
- average run duration.
- retry rate.
- replay rate.

### Semantic authority

- plan compile count/failure rate.
- action policy allow/block/gate counts.
- human gate wait/approve/deny counts.
- provenance verify failures.

### Model gateway

- model calls by alias/purpose.
- provider attempts and fallback count.
- streaming starts/completions/interruptions.
- quota status and quota exceeded count.
- usage/cost by project/model alias.

### Evidence spine

- unresolved edge count.
- downstream evidence fetch failures.
- average evidence graph resolution time.

## SLO starter set

| SLO | Target | Notes |
|---|---:|---|
| Local stack readiness | 99% in CI-controlled runs | alpha/local only |
| Cohesion smoke pass | 100% for release cut | release gate |
| Restart survival pass | 100% for release cut | release gate |
| Evidence spine resolution | 95% resolved for expected graph edges | unresolved edges explicit |
| Run completion for fake provider path | 99% | excludes intentional human-gate wait |
| Conexus fallback path | 100% in fake fallback smoke | release gate |

## Incident runbook starters

### Run stuck waiting

1. Fetch Allagma run detail.
2. Fetch run events.
3. Extract humanGateId / Kanon decision id.
4. Fetch Kanon semantic graph by humanGateId.
5. Confirm waiting/approved/denied state.
6. Resume or mark operator action required.

### Model call failed

1. Extract modelCallId and routeDecisionId.
2. Fetch Conexus model-call evidence.
3. Check provider attempts and fallback chain.
4. Check quota status.
5. Classify retryable/terminal.

### Semantic decision unavailable

1. Extract decisionId from run event.
2. Fetch Kanon decision record.
3. Fetch provenance.
4. Run verify.
5. If missing, mark evidence edge unresolved and inspect traceId.

## Production gap

The above is still pre-production unless metrics are exported to a managed backend, alerts are configured, and incident ownership is assigned.
