# 11 — Risk register

| Risk | Impact | Mitigation |
|---|---|---|
| Creating another parallel doc layer | High | `docs/learn/INDEX.md` must be canonical and old docs must link to it or be marked stale |
| Commands drift from scripts | High | Add command existence check and require command evidence in guide frontmatter |
| Generated artifacts manually copied | Medium | Link to generated files only; explain interpretation in prose |
| Stale CURRENT_STATE docs treated as truth | High | Audit matrix must classify stale/current before guide writing |
| Cross-repo ownership blurred | High | Boundary glossary and architecture map must be reviewed first |
| UX docs diverge from ontogony-ui contracts | Medium | `15_UI...` must link to `ontogony-ui/docs/*` contracts |
| Production readiness overstated | High | Use release/readiness wording precisely; cite evidence artifacts |
