# Deferred items register policy

A listed robustness PR must have one of these states:

- implemented;
- implemented as partial with named follow-up;
- deferred with rationale;
- superseded with evidence.

Current deferred candidates:

- PR-PLAT-003 — typed HTTP client support. Conexus uses existing integration HTTP pipeline successfully; no current platform pressure.
- PR-PLAT-012 — durable quota ledger design. Conexus has a Conexus-shaped EF quota ledger; do not generalize until another consumer creates pressure.
