# Implementer prompt

We are implementing a cross-repo Ontogony .NET release-gate package covering:

- SYSTEM-ALPHA-007 — full locked-runtime release gate
- SYSTEM-ALPHA-008 — scheduled full-system cohesion
- PLATFORM-REL-001 — package-mode release train

Repos:

- uridolan77/ontogony-platform
- uridolan77/kanon-dotnet
- uridolan77/conexus-dotnet
- uridolan77/allagma-dotnet

Important boundaries:

- Allagma owns runtime lock and system cohesion.
- Platform owns shared release mechanics and operator docs.
- Kanon owns semantic contract gates.
- Conexus owns capacity baseline.
- Do not enable real external tool execution.
- Do not label this as production readiness.
- Do not change Kanon `/ontology/v0` contracts unless an explicit contract migration is included.

Implementation order:

1. Add evidence schemas and validators.
2. Add package-mode release train command and summary validator.
3. Add scheduled cohesion command/workflow.
4. Add locked runtime release command/workflow.
5. Run package-only, cohesion-only, then full locked release.
6. Commit closeout evidence.

Acceptance:

- one command emits a validated release evidence bundle;
- scheduled workflow runs full scenario set;
- package-mode release train proves package consumption;
- no secrets in artifacts;
- release evidence only allowed in Locked mode.
