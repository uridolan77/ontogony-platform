# P1 — FE-LIVE-SMOKE-002

## Objective

Increase Docker-live frontend confidence beyond read-only overview smoke.

## Scope

Safe local-stack browser smoke only. No real external provider execution. No destructive mutation.

## Candidate flow

1. Start Docker-local stack.
2. Open frontend.
3. Configure local operator settings.
4. Visit command center, Conexus observability/model calls, Kanon decisions/provenance, Allagma run detail/evidence, and Evidence Spine lookup/export.
5. Assert live badges, at least one service response, no fixture-only banners on live routes, safe redaction, evidence/export panel renders.

## Acceptance

- Playwright spec exists.
- Command documented.
- Evidence file records status.
- Failures are clear and actionable.
