# SETTINGS-SECURITY-UX-001 Cursor Package

Start with:

```text
00_CURSOR_MASTER_PROMPT.md
```

Then implement tasks in order:

```text
cursor-tasks/001-credential-source-taxonomy.md
cursor-tasks/002-warning-consolidation.md
cursor-tasks/003-actor-presets.md
cursor-tasks/004-kanon-wording.md
cursor-tasks/005-redaction-preview.md
cursor-tasks/006-diagnostics-privacy.md
cursor-tasks/007-actor-provenance-labels.md
cursor-tasks/008-execution-posture-model-naming.md
```

Primary repo: `ontogony-frontend`.

Secondary repo: `ontogony-ui` if reusable components belong there.

Do not enable real external execution. Do not introduce production IAM. Do not expose raw secrets.
