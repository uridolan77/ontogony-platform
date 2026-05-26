# Conexus Cursor Prompt

Implement Conexus support for `ONTOGONY-SKILL-OPTIMIZATION-SPINE-001`.

1. Inspect current model routing, provider abstraction, fake provider, admin auth, model-call metadata, and route decision docs.
2. Add explicit target vs optimizer model-call metadata.
3. Add skill injection metadata to target model calls.
4. Add deterministic fake optimizer output for bounded SkillEdit proposals.
5. Add skill binding resolver or stub integration sufficient for local fixtures.
6. Add tests proving skill-injected calls record skillVersionId/contentHash/bindingId.
7. Add tests proving normal inference has zero optimizer calls.
8. Update docs and route inventories/OpenAPI if routes are added.

Do not make optimizer calls part of normal inference.
