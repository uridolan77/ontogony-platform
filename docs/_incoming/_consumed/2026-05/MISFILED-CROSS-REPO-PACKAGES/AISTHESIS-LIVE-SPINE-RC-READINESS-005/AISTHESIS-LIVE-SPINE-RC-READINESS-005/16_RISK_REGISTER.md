# Risk register

| Risk | Severity | Mitigation |
|---|---:|---|
| Live proof becomes stale | High | Include repo refs, rerun date, and exact artifact paths |
| Fixture passes but live producers drift | High | CI smoke and live proof reruns |
| Aisthesis becomes orchestrator | High | Enforce boundary doc |
| Required-edge rules drift from producer IDs | High | Cross-repo alignment matrix and tests |
| LES-002 partial ignored | Medium | Explicit analysis / accepted partial doc |
| Full Release gates skipped due file locks | Medium | Stop APIs and rerun clean |
| Production IAM overclaimed | High | Lock decision must mark IAM state |
| Retention deletion breaks audit | High | Gate only, no destructive API without tombstones |
| OTel exports sensitive payloads | High | Redaction policy before export |
| Frontend shows demo evidence as live | Medium | Frontend handoff forbids fabricated evidence |
