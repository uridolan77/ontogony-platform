# Integration outbound additional headers

**Date:** 2026-05-16  
**Package:** `Ontogony.Http`

## Change

`IntegrationOutboundState` and `IntegrationClientCallOptions` accept optional `AdditionalHeaders` propagated by `IntegrationHeadersDelegatingHandler` without overwriting existing request headers.

## Consumers

Allagma uses this for `X-Allagma-Run-Id` (SYS-COH-005). Product-specific header names remain in consumer repos.

## Migration

Additive only. Rebuild consumers referencing `Ontogony.Http`.
