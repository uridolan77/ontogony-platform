# Manual QA checklist — KANON-CONSOLE-POLISH-001

Run the local console with the normal local stack and inspect these pages.

## 1. Settings → Kanon semantic settings health

Check:

- Kanon base URL is visible.
- Ontology ID/version are visible.
- Seed match is visible.
- Actor id and roles are visible.
- Domain-pack read and provenance read grants are visible.
- Role guidance is clear.
- There is no repeated wall of credential warnings.
- Stored credential source does not show `unknown source` when the source is known.

## 2. Kanon → Conexus Assistance

Check:

- Default JSON sample has no secret-looking fields.
- Allowed fields do not include secret-looking fields.
- Force-redact fields are empty or safe by default.
- Redaction preview appears before submit or is available near submit.
- Assistance remains marked draft-only and non-authoritative.
- After a request, decision/evidence links are contextual.

## 3. Kanon → Domain Packs

Check:

- Inventory counts are understandable.
- `Packs on disk` and `Active ontology versions` are not conflated.
- Generated/test-looking versions do not look like normal production packs.
- Selected pack state is clear.
- Disabled actions are visibly disabled.
- Each disabled action explains why.
- Lifecycle timeline is understandable.
- Simulation buttons/outputs are clearly simulation-only.

## 4. Kanon → Review Queue

Check:

- Empty state is not vague.
- Partial state has reason.
- Role/auth issue is distinguishable from empty queue.
- Evidence links are contextual when available.

## 5. Kanon → Policies

Check:

- Empty/partial state has reason.
- Policy decision/evidence links are contextual.
- Operator can tell whether data is live, fallback, fixture, or unavailable.

## 6. Accessibility smoke

Check with keyboard:

- Disabled action reasons are reachable or visible.
- Copy buttons have names and feedback.
- Evidence links have meaningful link text.
- Redaction preview is readable without relying on color alone.

## 7. Regression smoke

- Start console.
- Refresh Kanon pages.
- Change no settings.
- No runtime errors in console.
- No warnings caused by undefined/null fields in the UI.
