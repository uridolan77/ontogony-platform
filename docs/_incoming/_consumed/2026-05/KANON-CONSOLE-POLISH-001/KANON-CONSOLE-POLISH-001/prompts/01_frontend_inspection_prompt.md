# Cursor frontend inspection prompt

Search the codebase for the following visible text and report the owning files/components:

```text
Conexus assistance workbench
Draft-only model assistance
Request draft assistance
Force-redact fields
Domain packs
Lifecycle history
Operator actor context
Domain-pack read
Provenance read
Review Queue
Policies
Evidence Spine
```

Then identify:

- data hooks/adapters used by those pages;
- shared UI components used for status badges/cards/empty states;
- existing tests;
- places where copy is duplicated.

Return an edit plan. Do not edit yet.
