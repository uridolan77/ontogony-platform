# Master Prompt — Cross-Repo Backend/Frontend Alignment

We are working across:
- uridolan77/ontogony-platform
- uridolan77/conexus-dotnet
- uridolan77/kanon-dotnet
- uridolan77/allagma-dotnet
- uridolan77/ontogony-frontend

Goal:
Align backend contracts with the Phase I `ontogony-frontend` release candidate. The frontend has passed `check:full` and should be treated as the executable operator workflow specification.

Rules:
- Do not invent routes.
- Do not remove frontend limitation banners until backend route + OpenAPI + tests exist.
- Use Allagma. Agentor is only the former historical name.
- Preserve redaction and evidence guarantees.
- Every new backend route must include OpenAPI, tests, auth/error behavior, and correlation metadata.
- Every frontend snapshot refresh must update OpenAPI provenance and pass `check:full`.

First action:
Read this package, then implement BFA-001 in `ontogony-platform`.
