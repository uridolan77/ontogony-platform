# SYS-TIGHT-003 — Operator audit journey v1

**Repo:** ontogony-frontend, ontogony-ui  
**Type:** operator UI  
**Priority:** P1

## Goal

Give operators one page/flow to audit a governed run across Allagma, Kanon, and Conexus.

## Scope

- Run timeline.
- Semantic decisions/provenance/human gates.
- Model calls/route decisions/provider attempts/quota/cost.
- Stream lifecycle summary.
- Failure diagnosis.

## Acceptance

- Docker-local Playwright smoke covers completed run, human gate, fallback, assistance, and evidence spine.
- UI marks Conexus assistance as `draft_only`.
- UI distinguishes authority boundaries.
