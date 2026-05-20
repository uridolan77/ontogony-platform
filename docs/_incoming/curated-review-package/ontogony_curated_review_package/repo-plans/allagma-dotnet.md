# allagma-dotnet plan

## Priority

Allagma should lead the cohesion sprint because it orchestrates the runtime loop.

## Keep

- Run lifecycle and event stream.
- Kanon.Client and Conexus.Client based integration.
- Model-purpose catalog and Conexus alias routing.
- Human-gate resume route.
- Audit/evaluation/baseline surfaces.
- Simulated/local sandbox execution while real external tools remain blocked.

## Implement next

1. `SYSTEM-COH-001`
2. `SYSTEM-E2E-001`
3. `SYSTEM-CTX-001`
4. `ALLAGMA-TOOL-TRUST-001`
5. `ALLAGMA-SANDBOX-OBS-001`
6. `ALLAGMA-EVIDENCE-001`
7. `ALLAGMA-STREAM-001`

## Do not do yet

Do not enable real network/filesystem/provider SDK tool execution from Allagma core. Keep real side effects unavailable until the trust model and side-effect replay rules are implemented and tested.
