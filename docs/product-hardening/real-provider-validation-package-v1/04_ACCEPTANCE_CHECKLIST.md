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

## RP-004

- [ ] frontend shows fake vs real-provider validation state
- [ ] provider errors are visible
- [ ] no UI secret entry
- [ ] fixture/live/degraded clarity preserved

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
