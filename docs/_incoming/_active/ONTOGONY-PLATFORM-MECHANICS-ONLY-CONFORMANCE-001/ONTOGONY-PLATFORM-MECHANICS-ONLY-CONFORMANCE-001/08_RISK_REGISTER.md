# Risk register

| Risk | Severity | Mitigation |
|---|---:|---|
| Platform starts absorbing product semantics | High | Mechanics-only proposal gate + no-product-semantics scan |
| Conformance harnesses become too abstract to be useful | Medium | Require runnable scripts and sample fixture outputs |
| Consumer repos diverge on headers/errors/idempotency | High | Schema registry + per-consumer conformance reports |
| Platform becomes runtime lock authority | High | Closeout non-claim; Allagma/system evidence remains runtime baseline authority |
| Schema changes break consumers | Medium | Versioned schemas + compatibility policy |
| False positives in semantic leak scanning | Medium | Maintain allowlist and owner-routing table |
| Harnesses require all services live | Medium | Support static/fixture mode and live mode separately |
| Observability naming becomes inconsistent | Medium | Meter naming contract + conformance script |
| Replay schema interpreted as replay runtime | High | Explicitly contract-only; product repos own execution semantics |
