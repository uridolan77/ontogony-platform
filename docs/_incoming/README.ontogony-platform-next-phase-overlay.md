# Ontogony.Platform next-phase overlay

This overlay is a planning/specification package for the next Ontogony.Platform phase after PR-PLAT-011.

It assumes the current repo state includes:

- package publishing workflow;
- Conexus package smoke;
- public API approval snapshots;
- dependency baseline validation;
- scoped CS1591 enforcement on the Conexus consumer surface;
- security/supply-chain workflows;
- in-memory non-durable startup warnings;
- generic secret-value resolver.

Primary next work:

1. release workflow parity + first tag publish proof;
2. real Conexus package-mode compatibility;
3. security workflow first-run evidence;
4. donor/incoming package hygiene;
5. docs accuracy pass;
6. deferred item register;
7. secret reference parser;
8. expanded warning coverage;
9. public API change checklist.

All implementation work should remain platform-neutral.
