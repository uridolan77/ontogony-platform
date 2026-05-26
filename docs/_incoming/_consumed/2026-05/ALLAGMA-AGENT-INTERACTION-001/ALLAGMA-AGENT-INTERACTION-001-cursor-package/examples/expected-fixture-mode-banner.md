# Expected fixture mode banner

When fixture replay is selected, the page must show:

```text
Demo fixture — not live evidence
```

Recommended detail:

```text
This session is loaded from static fixture data. It is useful for UI replay only and does not prove Allagma, Kanon, or Conexus connectivity.
```

Forbidden claims in fixture mode:

```text
System connected
Trace bridge verified
All services resolved this run
Ready for release
```

Fixture exports must include:

```json
{
  "mode": "fixture_replay",
  "dataSource": "fixture",
  "isLiveEvidence": false
}
```
