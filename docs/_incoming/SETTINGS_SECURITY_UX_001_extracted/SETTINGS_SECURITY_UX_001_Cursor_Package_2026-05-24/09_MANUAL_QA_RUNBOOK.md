# Manual QA Runbook

## Prerequisites

Run local stack:

```powershell
# Ports expected by current operator console
# Kanon:   http://localhost:5081
# Conexus: http://localhost:5082
# Allagma: http://localhost:5083
```

Open the operator frontend.

## QA 1 — Settings credentials

1. Open Operator Settings.
2. Confirm there is one local credential warning group.
3. Confirm each credential row has:
   - configured/not configured
   - source
   - scope
   - no raw value
4. Confirm no visible text says `unknown source`.

Expected: pass.

## QA 2 — Actor presets

1. Select **Read-only semantic authority**.
2. Confirm roles become `Auditor, ProvenanceReader`.
3. Save.
4. Open Kanon Overview.
5. Confirm current actor/roles are shown.
6. Confirm write capabilities are not misleadingly granted.

Then:

1. Return to Settings.
2. Select **Local admin/operator**.
3. Confirm roles become `Admin`.
4. Confirm the UI no longer says Docker-local expects read-only as if it were the active profile.

Expected: no contradiction.

## QA 3 — Kanon wording

Inspect Kanon Overview, Domain Packs, Ontology Versions, Source Bindings.

Forbidden text:

```text
Allagma defaults
Kanon trusts headers
```

Expected: absent.

## QA 4 — Redaction preview

1. Open Kanon / Conexus Assistance.
2. Load sample context.
3. Confirm sample contains no secret-looking fields.
4. Add a test field if UI supports editing:
   - `apiKey`
   - `connectionString`
   - `token`
5. Verify preview removes those fields before assistance call.

Expected: outbound preview contains no secret-like fields or values.

## QA 5 — Diagnostics export

1. Open Settings.
2. Click diagnostics export preview/copy/download.
3. Confirm privacy notice is visible.
4. Inspect exported JSON.

Expected:

```json
"containsRawSecrets": false
"redactionApplied": true
```

No raw API keys.

## QA 6 — Actor labels in evidence/run views

1. Open a recent run or Evidence Spine graph.
2. Confirm actor labels specify:
   - current operator actor
   - historical run actor
   - service actor if present

Expected: no unlabeled ambiguous actor identity.

## QA 7 — Execution posture

1. Open Home.
2. Open Settings.
3. Open Allagma overview/run page.
4. Confirm posture is compact:
   - real external blocked
   - sandbox disabled
   - kill switch status with severity/scope

Expected: no repeated long warning blocks unless expanded.
