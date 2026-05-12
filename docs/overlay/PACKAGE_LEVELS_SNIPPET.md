# Package levels snippet

Add these package rows/edges to `docs/architecture/package-levels.md` and `scripts/validate-package-levels.ps1`.

```powershell
'Ontogony.Logging'          = @('Ontogony.Observability')
'Ontogony.Redaction'        = @()
'Ontogony.Secrets'          = @('Ontogony.Redaction')
'Ontogony.Quotas'           = @('Ontogony.Primitives')
'Ontogony.Replay.Contracts' = @()
```

Recommended docs placement:

```text
Level 1 — Service mechanics:
  Ontogony.Logging
  Ontogony.Redaction
  Ontogony.Secrets

Level 2 — Event, consistency, and resource-control mechanics:
  Ontogony.Quotas

Level 3 — AI runtime and replay mechanics:
  Ontogony.Replay.Contracts
```
