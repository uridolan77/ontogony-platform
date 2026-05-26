# Cursor prompt — 06 Settings and Evaluation adoption

Migrate Settings and Evaluation surfaces to the shared taxonomy.

## Settings

- Normalize secret source labels: `session`, `local`, `default`, `env-injected`, `not set`.
- Avoid `unknown source` unless paired with reason.
- Current operator actor and historical run actor must be separately labeled.
- Browser diagnostics should be marked as client diagnostics and privacy-reviewed where appropriate.

## Evaluation

- Do not overstate quality evidence when provider metadata, tokens, dataset ID, or baseline comparison are missing.
- Use evidence/data-source/authority statuses for evaluation rows.
- If trend is current-page preview, label it as such.
- All-pass rows with missing metadata should be `limited evidence`, not broad quality proof.
