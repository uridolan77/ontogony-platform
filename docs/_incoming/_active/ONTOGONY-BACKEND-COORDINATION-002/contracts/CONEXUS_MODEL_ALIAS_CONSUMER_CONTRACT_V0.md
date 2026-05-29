# Conexus model alias — consumer contract v0

**Package:** `ALLAGMA-CONEXUS-MODEL-ALIAS-001`  
**Owner:** `conexus-dotnet`  
**Consumers:** `allagma-dotnet` (primary), future Metabole advisory calls

## Rule

Consumers request models by **alias** only. Conexus resolves alias → route → provider → model.

Consumers must **not** send provider model IDs (e.g. `gpt-4o-mini`) in governed product paths.

## Allagma mapping

```text
Allagma execution purpose (e.g. summarize-player-risk)
  → AllagmaModelPurposeRoute.ConexusModelAlias (config)
  → Conexus.Client chat request with model = alias
  → Conexus route decision + modelCallId
```

## Manifest

Machine-readable snapshot:

```text
allagma-dotnet/docs/system/conexus-model-alias-manifest.snapshot.json
```

Must list every purpose used in default/dev config and CI smoke.

## Conexus responsibilities

- Register aliases in admin/config store
- Record route decisions and provider evidence
- Expose alias list for consumer manifest validation

## Allagma responsibilities

- Configure purposes → aliases in `appsettings` / options
- Persist `ConexusModelAlias` on runs for replay
- Never branch on provider name

## Validation

```powershell
# Allagma src must not contain gpt-* (excluding archived docs)
rg "gpt-[0-9]" allagma-dotnet/src/ --glob "!*Tests*"
```

Tests use aliases like `test-alias`, `risk-summary-v0`.
