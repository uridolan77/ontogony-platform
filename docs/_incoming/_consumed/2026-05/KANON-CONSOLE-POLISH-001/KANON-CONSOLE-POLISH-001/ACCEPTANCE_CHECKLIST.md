# Acceptance checklist — KANON-CONSOLE-POLISH-001

## Assistance workbench

- [ ] Default context contains no `apiKey`, `secret`, `token`, `password`, or other secret-like field/value.
- [ ] Default allowed fields contain only non-sensitive fields.
- [ ] Force-redacted fields are explained and previewed before submit.
- [ ] Draft output is explicitly labeled `draft_only` / non-authoritative.
- [ ] Human review requirement remains visible.
- [ ] Successful draft response links to Kanon decision/evidence when IDs exist.
- [ ] Empty state is calm and clear.

## Domain packs

- [ ] Packs on disk are not conflated with active ontology versions.
- [ ] Active ontology versions are labeled as ontology versions, not necessarily user-facing packs.
- [ ] Generated/test-looking pack names are grouped, filtered, or caveated if detectable.
- [ ] Selected pack/version state is visually clear.
- [ ] Unavailable lifecycle actions are disabled inline.
- [ ] Every disabled lifecycle action has a reason.
- [ ] Lifecycle timeline labels are coherent and do not invent unsupported backend states.
- [ ] Diff/impact/migration simulation outputs are clearly simulation-only.

## Settings / semantic health

- [ ] Kanon settings health remains truthful: grants, roles, ontology version, and base URL are visible.
- [ ] Role guidance distinguishes local admin from read-only semantic authority.
- [ ] Repeated credential warnings are reduced.
- [ ] Credential source label is not `unknown source` when storage mode is known.
- [ ] Health payload warnings are not hidden.

## Review Queue / Policies

- [ ] Partial state has explicit reason code or reason copy.
- [ ] Empty state gives next action.
- [ ] Auth/role missing state is distinct from empty data.
- [ ] Missing backend/generated-client state is distinct from empty data.

## Evidence links

- [ ] Decision links say what decision they open.
- [ ] Evidence Spine links say what identifier they open.
- [ ] Copy buttons have accessible labels and success feedback.
- [ ] Link grouping does not duplicate the same target without context.

## Tests

- [ ] Unit/render test prevents secret-like assistance defaults.
- [ ] Unit/render test covers redaction preview.
- [ ] Unit/render test covers disabled domain-pack action reasons.
- [ ] Unit/render test covers domain-pack inventory separation.
- [ ] Unit/render test covers Review Queue/Policies partial reason rendering.
- [ ] Existing tests pass.

## Documentation / PR summary

- [ ] PR summary lists pages touched.
- [ ] PR summary lists files changed.
- [ ] PR summary lists commands run.
- [ ] PR summary lists backend follow-ups, if discovered.
- [ ] No backend scope creep occurred.
