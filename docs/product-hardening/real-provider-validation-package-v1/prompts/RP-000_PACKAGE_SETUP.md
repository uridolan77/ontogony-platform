# RP-000 — Package Setup Prompt

```text
We are starting RP-000 — Real provider validation package setup.

Repo:
- uridolan77/ontogony-platform

Source ZIP:
- docs/_incoming/Ontogony-Real-Provider-Validation-Package-v1.zip

Target unpack path:
- docs/product-hardening/real-provider-validation-package-v1/

Current state:
- PRODUCT-MANUAL-QA-002R1 is PASS.
- Docker-local fake-provider system is validated.
- Production readiness is NOT started.

Goal:
Register this package as the next controlled validation program.

Boundary:
- Docs/package setup only.
- No runtime source changes.
- No workflow changes.
- No real provider call yet.
- No provider secrets.
- No production-readiness claim.

Tasks:
1. Copy ZIP to docs/_incoming/.
2. Unpack to docs/product-hardening/real-provider-validation-package-v1/.
3. Validate 00_MANIFEST.json parses.
4. Confirm prompts exist.
5. Add docs/evidence/RP_000_PACKAGE_SETUP_EVIDENCE.md.
6. Add pointer from docs/product-hardening/README.md.
7. Do not start RP-001 yet.

Acceptance:
- package unpacked
- manifest valid
- evidence added
- no runtime changes
- no workflow changes
- no secrets
- not production readiness

Suggested branch:
docs/rp-000-real-provider-package-setup

Suggested commit:
docs(product): add real provider validation package
```
