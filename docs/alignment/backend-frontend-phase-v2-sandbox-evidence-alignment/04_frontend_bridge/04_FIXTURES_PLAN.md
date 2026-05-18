# Fixtures Plan

Add frontend fixtures for:

1. Full successful local sandbox execute: dry-run ledger row, execute ledger row completed, marker relative path, effect fingerprint, executor ref, Started, Completed.
2. Replay-safe skip: completed execute ledger row, ReplaySkipped, no second completed event required.
3. Blocked execute: blocked event, reason, no execute ledger completed row.
4. Failed execute: started event, failed event, failed execute ledger row.
5. No sandbox evidence: old/normal run without sandbox evidence.

Fixtures may simulate backend payloads, but UI must clearly treat them as fixture/demo data, not live backend claims.
