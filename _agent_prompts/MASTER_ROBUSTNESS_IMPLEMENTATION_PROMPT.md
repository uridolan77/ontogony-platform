# Master robustness implementation prompt

Implement one PR spec at a time.

Read:
1. docs/reviews/OPEN_ISSUES_REGISTER.md
2. conexus-dotnet/docs/planning/robustness/CX_ROBUSTNESS_SEQUENCE.md
3. ontogony-platform/docs/planning/robustness/PLAT_ROBUSTNESS_SEQUENCE.md

Rules:
- Ontogony owns mechanics.
- Conexus owns gateway semantics.
- Every PR includes tests and BUILD_VALIDATION/changelog/doc updates where relevant.
- No raw secrets/prompts/completions in logs/telemetry/exceptions by default.
