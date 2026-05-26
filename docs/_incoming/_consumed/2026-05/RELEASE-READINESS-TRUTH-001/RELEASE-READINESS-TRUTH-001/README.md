# RELEASE-READINESS-TRUTH-001

Cursor implementation package for making the Ontogony console release-readiness surface truthful, non-misleading, and operationally useful.

Primary area:
- `/system/release-readiness`
- route readiness scorecard generated from `docs/generated/operator-release-readiness.json`
- release posture summary cards
- route-level data-source/status semantics
- generated-artifact freshness and validation copy
- shared readiness/status copy used by System pages when relevant

Core thesis:

> The release-readiness page must stop treating generated route coverage as release readiness. It should clearly separate generated route inventory, live backend validation, semantic/operator readiness, artifact freshness, and unresolved gaps.

This package is intentionally frontend/operator-console-first. Backend work is allowed only as a documented follow-up unless the current repository already exposes the needed live validation routes.
