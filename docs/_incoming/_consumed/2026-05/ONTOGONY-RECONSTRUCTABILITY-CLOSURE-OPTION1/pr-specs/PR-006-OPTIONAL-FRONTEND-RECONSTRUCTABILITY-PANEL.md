# PR-006 — Optional frontend reconstructability panel

## Start condition

Do not start this PR until PR-004 cross-service golden trace is green.

## Repo focus

```text
C:\dev\ontogony-frontend
C:\dev\ontogony-ui
```

## Goal

Expose backend-provided reconstructability results to operators.

## UI requirements

Panel should show:

```text
PASS/WARN/FAIL
decision event kind
service of origin
severity
weak/missing properties
fragment links
related run/model/decision IDs
trace/correlation IDs
```

## Boundary

Frontend must not compute classifier results.

Allowed:

```text
render backend results
group by service/severity/status
link to Allagma/Kanon/Conexus detail pages
redacted JSON viewer
safe export
```

Forbidden:

```text
client-side F/P/S/O grading
inventing missing fragments
raw prompt/completion display
```

## Suggested routes

```text
/system/evidence-spine/:traceId/reconstructability
/allagma/runs/:runId/reconstructability
/conexus/model-calls/:modelCallId/reconstructability
/kanon/reconstructability/:decisionEventId
```

## Acceptance criteria

```text
Backend result drives UI.
No raw sensitive fields rendered.
UI handles PASS/WARN/FAIL and partial evidence.
Tests cover at least one Allagma and one Conexus fixture.
```
