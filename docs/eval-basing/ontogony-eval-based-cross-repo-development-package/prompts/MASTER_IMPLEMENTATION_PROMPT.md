# Master Implementation Prompt — Eval-Based Cross-Repo Phase

You are implementing the Ontogony eval-based topology and baseline hardening phase.

Repos in scope:

- `ontogony-platform`
- `kanon-dotnet`
- `conexus-dotnet`
- `allagma-dotnet`

Rules:

1. Allagma replaces any old execution-layer naming. Use Allagma everywhere.
2. Do not add product semantics to Ontogony.Platform.
3. Do not move semantic authority into Conexus.
4. Do not move provider routing into Kanon.
5. Do not enable real external tool execution.
6. Default to `single_workflow`.
7. Add topology expansion only after eval evidence exists.
8. Every PR must include tests, docs, and evidence.

Start by reading:

```text
README.md
00_EXECUTIVE_BRIEF.md
02_TARGET_ARCHITECTURE.md
03_CROSS_REPO_PHASE_ROADMAP.md
04_ACCEPTANCE_MATRIX.md
05_DEPENDENCY_AND_BOUNDARY_RULES.md
```

Implement PRs in the order specified in `10_IMPLEMENTATION_SEQUENCE.md`.

For each PR, produce:

```text
summary
files changed
tests run
evidence artifacts
known limitations
follow-up PRs
```
