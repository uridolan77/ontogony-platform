# 04 — Acceptance Checklist

## RP-001

- [ ] real-provider disabled by default
- [ ] fake provider remains default
- [ ] budget caps documented
- [ ] kill switch documented
- [ ] CI real-provider calls forbidden
- [ ] no secrets committed

## RP-002

- [ ] Conexus local real-provider mode explicit opt-in
- [ ] missing/invalid key classified
- [ ] one small real call succeeds or external failure is classified
- [ ] route/model-call evidence persisted and redacted
- [ ] fake provider regression passes

## RP-003

- [ ] Allagma guided flow can use Conexus real-provider path
- [ ] run/eval/evidence persists
- [ ] trace/correlation survives
- [ ] Kanon behavior unchanged
- [ ] fake-provider regression passes

## RP-003A (follow-up — live completion)

- [ ] provider_transport_error root cause documented
- [ ] Docker runtime TLS trust fixed (if applicable)
- [ ] one live real-provider completion succeeds
- [ ] eval export on successful real run
- [ ] fake-provider regression after kill switch

## RP-004

- [x] frontend shows fake vs real-provider validation state
- [x] provider errors are visible
- [x] no UI secret entry
- [x] fixture/live/degraded clarity preserved

## RP-005

- [ ] manual results recorded
- [ ] call count/token/cost notes recorded if available
- [ ] evidence redacted
- [ ] fake-provider regression passes

## Closeout

- [ ] scorecard
- [ ] limitations
- [ ] next options
- [ ] explicit not production readiness
