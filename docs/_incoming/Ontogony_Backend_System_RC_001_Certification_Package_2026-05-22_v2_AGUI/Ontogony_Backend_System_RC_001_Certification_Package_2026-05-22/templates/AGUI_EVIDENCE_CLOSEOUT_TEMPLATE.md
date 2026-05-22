# SYSTEM-RC-001F — AG-UI / Agent Interaction Spine Certification Evidence

**Date:** `<YYYY-MM-DD>`  
**Runtime lock:** `<SYSTEM-ALPHA-007 or current RC id>`  
**Repos:**

| Repo | Commit | Status |
| --- | --- | --- |
| ontogony-platform | `<sha>` | `<PASS/FAIL>` |
| allagma-dotnet | `<sha>` | `<PASS/FAIL>` |
| kanon-dotnet | `<sha>` | `<PASS/FAIL>` |
| conexus-dotnet | `<sha>` | `<PASS/FAIL>` |

## Verdict

```text
PASS / FAIL / DEGRADED
```

## Commands run

```powershell
# paste exact commands here
```

## Artifact index

| Artifact | Path | Verdict |
| --- | --- | --- |
| Golden JSONL export | `artifacts/agui/<timestamp>/golden-run-interaction-events.jsonl` |  |
| Schema validation | `artifacts/agui/<timestamp>/golden-run-interaction-events.schema-validation.json` |  |
| SSE stream transcript | `artifacts/agui/<timestamp>/golden-run-interaction-stream-transcript.txt` |  |
| Resume report | `artifacts/agui/<timestamp>/golden-run-interaction-stream-resume.json` |  |
| Redaction report | `artifacts/agui/<timestamp>/agui-redaction-report.json` |  |
| Evidence link report | `artifacts/agui/<timestamp>/agui-evidence-link-resolution-report.json` |  |

## Event sequence summary

| Sequence | Event type | Owner | Evidence refs | Notes |
| --- | --- | --- | --- | --- |
| 1 |  |  |  |  |

## Redaction result

| Check | Result |
| --- | --- |
| No raw prompts |  |
| No raw completions |  |
| No API keys / tokens |  |
| No connection strings |  |
| No unapproved sensitive payloads |  |

## Known gaps

- `<gap>`

## Closeout statement

The AG-UI backend spine is certified for alpha RC only if all required artifacts pass and no raw sensitive data appears in the event stream.
