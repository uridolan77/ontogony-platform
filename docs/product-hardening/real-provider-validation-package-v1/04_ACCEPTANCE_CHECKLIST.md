# 04 — Acceptance Checklist

**Package status:** **CLOSED / PASS** (`RP-CLOSEOUT-001`, 2026-05-19)

## RP-001

- [x] real-provider disabled by default
- [x] fake provider remains default
- [x] budget caps documented
- [x] kill switch documented
- [x] CI real-provider calls forbidden
- [x] no secrets committed

## RP-002

- [x] Conexus local real-provider mode explicit opt-in
- [x] missing/invalid key classified
- [x] one small real call succeeds or external failure is classified
- [x] route/model-call evidence persisted and redacted
- [x] fake provider regression passes

## RP-003

- [x] Allagma guided flow can use Conexus real-provider path
- [x] run/eval/evidence persists
- [x] trace/correlation survives
- [x] Kanon behavior unchanged
- [x] fake-provider regression passes

## RP-003A (follow-up — live completion)

- [x] provider_transport_error root cause documented
- [x] Docker runtime TLS trust fixed (if applicable)
- [x] one live real-provider completion succeeds
- [x] eval export on successful real run
- [x] fake-provider regression after kill switch

## RP-004

- [x] frontend shows fake vs real-provider validation state
- [x] provider errors are visible
- [x] no UI secret entry
- [x] fixture/live/degraded clarity preserved

## RP-005

- [x] manual results recorded
- [x] call count/token/cost notes recorded if available
- [x] evidence redacted
- [x] fake-provider regression passes

## Closeout

- [x] scorecard
- [x] limitations
- [x] next options
- [x] explicit not production readiness
