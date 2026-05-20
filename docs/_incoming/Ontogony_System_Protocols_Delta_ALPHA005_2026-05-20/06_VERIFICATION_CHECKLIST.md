# Verification checklist

## Before starting

- [ ] Confirm sibling repos are present under `C:\dev`.
- [ ] Confirm `SYSTEM-ALPHA-005` lock file is readable.
- [ ] Confirm current branch SHAs for platform, Conexus, Kanon, Allagma, frontend, UI.
- [ ] Confirm old `_incoming` packages are treated as source material only.

## Per-repo

### ontogony-platform

- [ ] `dotnet test`
- [ ] Docker-local compose validates service build.
- [ ] Protocol registry schema validates.
- [ ] Stale incoming-package validator reports old stale package findings.
- [ ] Evidence index points to current Alpha-005 and moving-main delta.

### allagma-dotnet

- [ ] `dotnet test Allagma.sln -c Release --filter "Category!=CrossRepo&Category!=PersistenceSmoke"`
- [ ] `scripts/validate-runtime-lock.ps1 -RequireEvidence -ReleaseMode`
- [ ] `scripts/run-system-cohesion-smoke.ps1 -StartServices -IncludeKanonAssistance -IncludeConexusFallback -IncludeStreamingEvidence`
- [ ] Restart survival PASS.
- [ ] Runtime posture tests PASS.
- [ ] Feature connection matrix audit PASS.

### kanon-dotnet

- [ ] `dotnet test Kanon.sln -c Release`
- [ ] Route inventory/OpenAPI parity PASS.
- [ ] Conexus assistance tests PASS.
- [ ] KANON-CONNECT evidence is indexed.

### conexus-dotnet

- [ ] `dotnet test Conexus.sln -c Release --filter "Category!=ExternalProviderSmoke&Category!=LoadSoak&Category!=PersistenceSmoke"`
- [ ] Model-call evidence operator flow tests PASS.
- [ ] Route preview and quota status tests PASS.
- [ ] Streaming contract tests PASS.
- [ ] Production exposure safety tests PASS or explicitly quarantined.

### ontogony-frontend

- [ ] `npm run check`
- [ ] FE live Docker smoke PASS.
- [ ] Evidence Spine Docker-live PASS.
- [ ] Route/catalog parity checks PASS.

## Baseline cut conditions

- [ ] B-012 closed or carried forward explicitly.
- [ ] Full system cohesion PASS.
- [ ] Restart survival PASS.
- [ ] Lock updated with exact SHAs.
- [ ] Evidence paths exist.
- [ ] No stale baseline labels.
- [ ] No obsolete Agentor references.
- [ ] Real external tool execution remains blocked.
