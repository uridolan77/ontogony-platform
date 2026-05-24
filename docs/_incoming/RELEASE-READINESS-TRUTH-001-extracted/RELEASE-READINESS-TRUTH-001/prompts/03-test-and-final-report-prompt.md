# Prompt 03 — Test and final report

Run the relevant checks for RELEASE-READINESS-TRUTH-001.

Recommended commands depend on repo conventions. Inspect `package.json` first.

Likely commands:

```bash
npm test -- release-readiness
npm run typecheck
npm run lint
npm run build
```

Use actual available scripts.

Final report format:

```text
RELEASE-READINESS-TRUTH-001 final report

Changed files:
- ...

Behavior changes:
- ...

Acceptance criteria satisfied:
- ...

Tests/checks run:
- command: result

Skipped/deferred:
- ...

Backend/live readiness follow-ups:
- ...
```

If a check fails for a pre-existing unrelated reason, separate it from new failures.
