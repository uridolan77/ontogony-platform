# Prompt 02 — Review RELEASE-READINESS-TRUTH-001 implementation

Review the implementation against the package.

Check:

1. Does the page still show unqualified `Ready / Partial / Gap` counts?
2. Can fixture-only rows be mistaken for release-ready rows?
3. Are generated artifact and live validation separated?
4. Is release-candidate posture separate from artifact route status?
5. Do partial/unknown/fallback rows show reasons and next actions?
6. Are data-source semantics centralized or scattered?
7. Are stale/future/invalid artifact timestamps handled?
8. Are tests meaningful and aligned with current repo conventions?
9. Did the implementation avoid backend invention?
10. Did it avoid unrelated rewrites?

Return:

```text
Verdict: pass / pass with follow-ups / fail
Issues found:
Acceptance criteria missed:
Tests reviewed:
Suggested fixes:
```
