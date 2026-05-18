# ENV-SETUP-001 — Workspace layout, settings, and environment docs

Repo: `ontogony-platform`

Add:

```text
docs/environments/local-operator-sanity/
  00_MANIFEST.json
  01_WORKSPACE_LAYOUT.md
  02_EXACT_SETTINGS.md
  03_MAIN_USE_FLOW.md
  04_STARTUP_MODES.md
  05_ACCEPTANCE_CHECKLIST.md
  06_TROUBLESHOOTING.md
  07_KNOWN_LIMITATIONS.md
```

Must document:

- exact `C:\dev\...` layout including `ontogony-ui`
- ports 5081/5082/5083
- fake/local provider first
- `Allagma__Evaluation__ManualWriteEnabled=true`
- no real external execution
- no production-readiness claim

Evidence:

```text
docs/evidence/ENV_SETUP_001_LOCAL_OPERATOR_SANITY_DOCS.md
```
