# First Version Definition

A first complete version means:

```text
A local operator can run Ontogony, understand posture, start a governed run,
inspect decisions and model-call evidence, handle gates, follow Evidence Spine,
and see that real external tools are blocked.
```

## Required operator flows

1. System posture / health.
2. Simple governed run.
3. Human gate approve.
4. Human gate deny.
5. Conexus fallback.
6. Kanon assistance draft-only.
7. Evidence Spine trace/explanation.
8. Sandbox/tool mode visibility.

## Required validation before RC

- All repo focused tests.
- Frontend `npm run check`.
- Docker-local system cohesion.
- Docker restart survival.
- Observability check.
- Evidence Spine Docker-live.
- Demo Playwright.
- Protocol/stale/real-tools validators.
