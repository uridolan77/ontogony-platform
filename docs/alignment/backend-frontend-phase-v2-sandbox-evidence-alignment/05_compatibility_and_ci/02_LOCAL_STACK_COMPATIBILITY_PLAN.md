# Local Stack Compatibility Plan

## Goal
Prove that backend contracts and frontend rendering agree.

## Minimal compatibility flow

1. Start Conexus, Kanon, Allagma with local/test configuration.
2. Run or load a known Allagma run with sandbox evidence.
3. Export audit bundle.
4. Verify `SandboxEvidence` fields.
5. Verify sandbox execute events.
6. Refresh frontend OpenAPI/client snapshot.
7. Run frontend checks.
8. Open/render run detail/audit surface.
9. Write compatibility report.

## Report fields

```json
{
  "schema": "ontogony-fba2-compatibility-report-v1",
  "generatedAt": "ISO",
  "services": [],
  "frontend": {},
  "allagmaRun": {
    "runId": "",
    "hasSandboxEvidence": true,
    "hasReplaySafeExecute": true,
    "events": []
  },
  "verdict": "PASS"
}
```
