# 05 — PR Sequence

| Order | Item | Repos | Purpose |
| --- | --- | --- | --- |
| 0 | `RP-000` | platform | Register package |
| 1 | `RP-001` | platform + conexus | Secret/budget/safety gates |
| 2 | `RP-002` | conexus + platform | Real-provider local mode |
| 3 | `RP-003` | allagma + platform | Real-provider guided flow |
| 3a | `RP-003A` | conexus + allagma + platform | Live completion after Docker TLS fix |
| 4 | `RP-004` | frontend + platform | Operator visibility |
| 5 | `RP-005` | platform | Manual execution |
| 6 | `RP-CLOSEOUT-001` | platform | Closeout |

## Rules

- Do not make real-provider mode default.
- Do not run real-provider tests in CI.
- Do not start RP-003 before RP-002 proves Conexus evidence.
- Do not close until fake-provider regression passes.
