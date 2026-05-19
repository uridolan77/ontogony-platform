# EVIDENCE-SPINE-008 — e2e browser and Docker verification

Goal:
Prove the evidence spine works in the browser with Docker-local fresh build provenance.

Repos:
- C:\dev\ontogony-frontend
- C:\dev\ontogony-platform

Precondition:
DOCKER-LOCAL-VERIFY-001 is available.

Tasks:

1. Add/extend e2e tests:
   - run ID lookup
   - eval ID lookup
   - model-call ID lookup
   - decision ID lookup
   - partial missing edge
   - export bundle

2. Add Docker-local manual script or checklist:
   - verify frontend provenance
   - seed/guided flow
   - capture IDs
   - resolve IDs in evidence spine
   - export bundle

3. Platform evidence:
   - docs/evidence/EVIDENCE_SPINE_008_E2E_BROWSER_VERIFICATION_EVIDENCE.md

4. Validation:
   - npm run typecheck
   - focused unit tests
   - relevant Playwright tests
   - verify-frontend-browser-provenance.ps1 -Build
   - manual browser check

Acceptance:
- browser proves paste-any-ID pattern for at least run/eval/model-call/decision
- Docker-local stale-bundle risk is controlled
