# GOVERNED-FAKE-E2E-001 Cursor Package

This package contains the implementation plan, contracts, tests, runbooks, and Cursor task breakdown for proving one **live governed fake-provider run** end-to-end across Ontogony.

Start with:

```text
00_CURSOR_MASTER_PROMPT.md
```

Then implement tasks in:

```text
cursor-tasks/
```

Primary proof:

```text
Allagma governed run
  -> Kanon decision
  -> Conexus fake model call
  -> Conexus route decision
  -> fake provider attempt
  -> Evidence Spine export
```

No real external execution. No real OpenAI. No fixture evidence counted as live proof.
