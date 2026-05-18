# Unpack Prompt

Use this prompt to unpack the package into `ontogony-platform`.

```text
We are updating the Ontogony eval planning package.

Repo:
- uridolan77/ontogony-platform

Current ZIP:
ontogony-platform/docs/_incoming/ontogony-eval-durability-to-first-sanity-current-package.zip

Task:
Unpack this ZIP into:

docs/planning/eval-durability-to-first-sanity-current/

Do not overwrite unrelated planning packages.
Preserve the old package at:
docs/_incoming/ontogony-eval-hardening-to-full-sanity-package.zip
as historical input.

After unpacking:
1. Add/update docs/planning/README.md or docs/alignment/README.md to point to this package as the active current plan.
2. State that the older incoming package is superseded by this current package.
3. Do not modify source code in this PR.
4. Do not start EVAL-DUR-001 yet.
5. Commit with:

docs: add current eval durability to first sanity package

Validation:
- confirm 00_MANIFEST.json exists
- confirm 01_CURRENT_STATE.md exists
- confirm 02_UNPACK_PROMPT.md exists
- confirm 03_NEXT_SEQUENCE.md exists
- confirm PR specs exist under 04_PR_SPECS/
```
