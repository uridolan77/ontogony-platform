# SYS-TIGHT-006 — Cross-service failure taxonomy adapter

**Repo:** ontogony-platform, allagma-dotnet, ontogony-frontend  
**Type:** shared taxonomy + adapter tests  
**Priority:** P1

## Goal

Normalize operator-facing failures without breaking service-specific public contracts.

## Scope

- Map Kanon, Conexus, Allagma errors to shared operator taxonomy.
- Surface retryability, downstream service, stage, trace/correlation.
- Add frontend failure banners.

## Acceptance

- Representative 400/403/404/409/503/provider/quota/idempotency failures map predictably.
- Public Kanon/Conexus contracts are unchanged.
