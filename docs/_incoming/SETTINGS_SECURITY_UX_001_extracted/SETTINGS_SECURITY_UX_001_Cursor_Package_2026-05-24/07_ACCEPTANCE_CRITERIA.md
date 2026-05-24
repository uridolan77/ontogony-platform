# Acceptance Criteria

## Settings page

- [ ] Shows exactly one local credential storage warning.
- [ ] No credential row displays raw secret values.
- [ ] No row says bare `unknown source`.
- [ ] Every credential row has source, scope, and configured/not configured status.
- [ ] Diagnostics export notice is visible before export/copy/download.
- [ ] Diagnostics export contains privacy metadata.
- [ ] Diagnostics export does not include raw secrets.

## Actor/role UX

- [ ] Settings includes actor role presets: read-only semantic authority, local admin, system service, custom.
- [ ] Current actor and roles are displayed clearly.
- [ ] Recommended Docker-local read-only profile is not presented as the active profile unless it is active.
- [ ] Kanon pages no longer say “Allagma defaults.”
- [ ] Kanon pages no longer say “Kanon trusts headers.”
- [ ] Kanon authorization copy explains local operator mode and non-local trusted boundary.
- [ ] Current operator actor and historical run/service actors are labeled distinctly.

## Conexus Assistance / redaction

- [ ] Sample context contains no `apiKey`, `secret`, `token`, `password`, `connectionString`, or secret-looking value.
- [ ] Allowed fields examples do not include secret-like field names.
- [ ] Redaction preview appears before sending assistance request.
- [ ] Redaction preview shows kept fields, removed fields, and reason codes.
- [ ] Redacted outbound payload can be inspected.
- [ ] Assistance output remains draft-only/advisory.

## Execution posture

- [ ] Real external execution remains blocked.
- [ ] Local sandbox disabled is visible but not alarming.
- [ ] Kill switch status includes severity and local-alpha scope.
- [ ] Runtime posture is not repeated in full on every page unless expanded.

## Model naming

- [ ] Settings distinguishes model purpose, Conexus alias, provider key, and provider model.
- [ ] Concrete provider model names are not shown as generic operator defaults.
- [ ] `summarize-player-risk → risk-summary-v0` style mappings are clear.

## Tests

- [ ] Unit tests cover credential-source taxonomy.
- [ ] Component tests cover role presets.
- [ ] Component tests verify no repeated credential warning.
- [ ] Tests verify forbidden UI phrases are absent from rendered primary settings/Kanon/assistance surfaces.
- [ ] Redaction tests verify secret-looking fields are removed.
- [ ] Diagnostics tests verify raw secrets are absent.
