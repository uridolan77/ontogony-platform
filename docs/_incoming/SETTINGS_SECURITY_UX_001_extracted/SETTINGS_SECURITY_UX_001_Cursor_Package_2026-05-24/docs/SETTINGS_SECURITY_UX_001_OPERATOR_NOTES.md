# SETTINGS-SECURITY-UX-001 Operator Notes

This sprint improves operator trust in Settings and security-adjacent UI.

## What changes

- Credentials show where they are stored, without showing raw values.
- Browser-local credentials produce one clear warning, not repeated warnings.
- Actor profiles are explicit.
- Kanon actor copy no longer implies production header trust.
- Conexus Assistance previews redaction before sending context.
- Diagnostics export states what client metadata it contains and confirms raw secrets are omitted.
- Current operator actor and historical run actor are labeled separately.
- Execution posture is compact and scoped to local alpha.

## What does not change

- Real external execution remains blocked.
- No production IAM is introduced.
- No new secret vault is introduced.
- Fake/local provider posture remains available for development.
