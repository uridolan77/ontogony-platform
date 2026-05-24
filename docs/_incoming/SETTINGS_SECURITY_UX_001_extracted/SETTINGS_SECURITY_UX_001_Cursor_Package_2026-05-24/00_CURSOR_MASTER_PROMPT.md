# SETTINGS-SECURITY-UX-001 — Cursor Master Prompt

You are working inside the Ontogony local-alpha operator system. Implement **SETTINGS-SECURITY-UX-001** as a focused truth/security/UX hardening sprint.

## Objective

Make the operator settings, credential handling, actor-role configuration, redaction UX, and local-security posture feel trustworthy, precise, and operator-grade.

The current console is functionally useful but too noisy and ambiguous. It repeats credential warnings, uses `unknown source` for secrets, defaults too often to `Admin`, shows confusing Docker-local actor guidance, allows secret-like examples in assistance pages, and does not clearly separate current operator actor, service actor, historical run actor, and runtime execution posture.

This sprint must not introduce production IAM or heavy security architecture. The goal is local-alpha/operator UX hardening with clean contracts and non-misleading copy.

## Evidence and diagnosis to respect

The attached reviews identify these recurring problems:

- Health/readiness warnings and Conexus not-ready details are visible from settings but not explained clearly.
- Credential warnings are repeated too often and secret source is sometimes `unknown source`.
- Docker-local role guidance contradicts current roles being sent: e.g. `Admin` sent while read-only guidance says `Auditor, ProvenanceReader`.
- Kanon pages mention “Allagma defaults” for actor settings, which is confusing.
- The UI says Kanon “trusts headers,” which is imprecise and teaches the wrong security posture.
- Current operator actor and historical run actor appear without enough distinction.
- Diagnostics export includes browser/client details and needs clear privacy labeling.
- Conexus Assistance sample context includes secret-looking values and secret-like field names.
- Redaction should be previewable and explainable before any AI assistance call.
- Model alias vs concrete model naming must remain precise.
- `No kill switch`, real external blocked, sandbox disabled, and local credentials need severity/scope treatment.

## Repositories likely involved

- `ontogony-frontend`
- `ontogony-ui`
- `allagma-dotnet` only if settings/diagnostics API needs support
- `kanon-dotnet` only if actor/auth capability metadata needs support
- `conexus-dotnet` only if provider/credential posture metadata needs support
- `ontogony-platform` only for shared contracts/errors/redaction primitives if already present

Prefer frontend-first improvements unless a small backend contract is required.

## Product principles

1. Do not overstate security.
2. Do not show secret material.
3. Do not use secret-looking examples.
4. Distinguish local alpha convenience from real security.
5. Distinguish current operator actor from historical run/service actors.
6. One warning in the right place is better than ten repeated warnings.
7. Use capability grants, not role-name guessing, whenever possible.
8. Keep model purpose, model alias, provider key, and provider model distinct.
9. Treat diagnostics export as potentially privacy-sensitive.
10. Make every ambiguous `unknown` labeled and actionable.

## Required deliverables

Implement, test, and document:

1. Settings credential-source taxonomy.
2. Collapsed credential warning model.
3. Actor role presets and clear current/recommended profile display.
4. Kanon actor wording cleanup.
5. Current actor vs historical actor labels across Evidence Spine / Agent Interaction / Kanon / Allagma surfaces.
6. Redaction policy and redaction preview for Conexus Assistance.
7. Removal of secret-looking examples from assistance sample context and allowed-fields examples.
8. Diagnostics export privacy labeling.
9. Execution posture / kill-switch severity cleanup.
10. Model alias/purpose naming precision in Settings.
11. Tests for settings copy, role presets, redaction preview, diagnostics privacy notice, and secret-source taxonomy.
12. Manual QA runbook.

## Strict non-goals

- Do not build production IAM/RBAC.
- Do not enable real external execution.
- Do not persist real secrets to repo files.
- Do not introduce new secret storage beyond existing browser/local/session/operator mechanisms.
- Do not weaken redaction to make UX simpler.
- Do not turn Kanon actor headers into an implied production trust model.

## Implementation method

Work in small commits:

1. Inventory current settings/security copy and components.
2. Add shared wording/taxonomy utilities.
3. Update Settings page.
4. Update Kanon actor panels.
5. Update Conexus Assistance/redaction flow.
6. Update diagnostics export labeling.
7. Update Evidence/Agent actor labels.
8. Add tests.
9. Run local smoke and document before/after.

## Completion definition

The console no longer feels like it is leaking debug/security ambiguity into the operator surface. It clearly says:

- what credential is set,
- where it is stored,
- what role profile is active,
- what local-alpha assumptions are being made,
- what actor is current vs historical,
- what is redacted before assistance,
- what diagnostics export contains,
- and why execution remains blocked.
