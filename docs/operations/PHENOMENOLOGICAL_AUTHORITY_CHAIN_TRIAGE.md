# Phenomenological authority chain triage

Operator runbook for Kanon → Aisthesis callback → Allagma phenomenological projection failures.

## Symptom: authority-status stuck in `pending_validation`

1. Confirm Kanon evaluated the mutation: `GET /ontology/v0/decision-records/by-trace/{traceId}`.
2. Verify `callbackUrl` on the evaluate request points to `/aisthesis/v1/memory/mutations/{mutationId}/validation-outcome/kanon-callback`.
3. Check Kanon logs for callback dispatch warnings (`Aisthesis validation callback returned` / `callback failed`).
4. Replay is idempotent: duplicate `callbackIdempotencyKey` with the same outcome is safe; conflicting replay returns 409.

## Symptom: `decisionRecordUrl` uses `/decisions/`

This is a graduation blocker. Links must be `/ontology/v0/decision-records/{decisionId}`. Fix Kanon coordinator `DecisionRecordUrl` emission or frontend normalizers.

## Symptom: `pending_review` applied a graph mutation

Authority bug. `pending_review` must not apply graph mutations. Inspect Aisthesis `GraphMutationApplicationService.RecordPendingReviewCallbackAsync` and Postgres ledger row for the mutation.

## Symptom: Allagma projection missing or `not_requested`

1. Confirm `Allagma:PhenomenologicalMemoryBridge:Enabled=true` and run reached a terminal state.
2. `GET /allagma/v0/runs/{runId}/phenomenological-projection` — 404 means no durable status row.
3. Check Aisthesis bridge health: `POST /aisthesis/v0/phenomenological-memory/projections` from Allagma logs.
4. Postgres mode: verify `allagma_phenomenological_projection_status` row exists for `run_id`.

## Live certification

```powershell
$env:KANON_BASE_URL = "http://localhost:5081"
$env:AISTHESIS_BASE_URL = "http://localhost:5085"
$env:ALLAGMA_BASE_URL = "http://localhost:5083"
$env:ALLAGMA_SERVICE_TOKEN = "allagma-dev-service-token-change-in-production"
cd ontogony-platform
powershell -File scripts/system/run-phenomenological-memory-authority-certification.ps1 -Mode Live
```

Optional override: `ONTOGONY_PHENOM_AUTH_LIVE_TRIGGER_URL` — POST endpoint returning `{ runId, mutationId, traceId?, expectedAuthorityStatus?, expectedProjectionStatus? }`.

## Raw payload leakage

Live certification runs `scripts/system/Test-PhenomenologicalAuthorityRawPayloadLeakage.ps1` on artifact output. Callback ledger and projection status must store fingerprints/metadata only.
