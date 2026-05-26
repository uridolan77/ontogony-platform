# Source findings — KANON-CONSOLE-POLISH-001

This issue map is derived from the raw Ontogony console review and supporting pasted console snapshots.

## High-confidence Kanon-specific issues

### KANON-FINDING-001 — Assistance page uses unsafe sample data

Observed console state:

```json
{
  "summary": "Operator review context (non-secret).",
  "apiKey": "secret-live-key"
}
```

The page also shows:

```text
Allowed fields: summary,apiKey
Force-redact fields: apiKey
```

Problem:

- The UI teaches operators to paste a secret-looking field into a model-assistance workbench.
- Even with force-redaction, the sample creates the wrong operational habit.
- Allowing and force-redacting the same field is conceptually confusing.

Required fix:

- Replace default context with non-secret fields.
- Remove `apiKey` from allowed fields examples.
- Add redaction preview before submit.
- Keep the draft-only/non-authoritative trust boundary prominent.

### KANON-FINDING-002 — Domain-pack inventory is semantically confusing

Observed console state:

```text
Packs on disk: 2
Active versions: 5
gaming-core@0.1.0, pg-lifecycle-..., pg-rehydrate-..., pg-switch-..., pg-unique-...
```

Problem:

- Operators can reasonably read this as “five active production domain-pack versions.”
- Several names look generated/test-like.
- The UI does not clearly distinguish disk inventory, persisted lifecycle rows, active ontology versions, selected pack, and test-generated artifacts.

Required fix:

- Separate inventory classes.
- Add filter/group for generated/test-looking pack versions if metadata supports it.
- Avoid normalizing generated names into the same prominence as `gaming-core@0.1.0`.

### KANON-FINDING-003 — Domain-pack action availability is noisy

Observed console state:

```text
Validate
Load
Promote
Deprecate
Unavailable actions
Load unavailable — This version is already active.
Promote unavailable — Version is already accepted or active.
Deprecate unavailable — Cannot deprecate the currently active pack version.
```

Problem:

- Actions appear visually available before the operator reads the unavailable reasons.
- Reasons are detached from controls.

Required fix:

- Disable unavailable buttons inline.
- Show reason adjacent to each disabled button or via accessible tooltip/helper text.
- Keep reasons visible enough for screenshot/debugging.

### KANON-FINDING-004 — Kanon settings health is correct but noisy

Observed console state:

- Kanon base URL is local.
- Ontology ID and version match seed.
- Actor id is `local-operator`.
- Actor roles are `Admin`.
- Domain-pack read and provenance read are granted.
- Text also says Docker-local expects `Auditor,ProvenanceReader` for read-only semantic authority.

Problem:

- Admin is convenient for local mutation, but the read-only Docker-local expectation is shown nearby and can confuse operators.
- Role requirements are useful but not framed as role presets/use cases.

Required fix:

- Add role preset explanation: `Local admin`, `Read-only semantic authority`, `System service`.
- Clarify that `Admin` is mutation-capable and `Auditor,ProvenanceReader` is read-only-oriented.
- Preserve truth: do not hide grants.

### KANON-FINDING-005 — Review Queue and Policies are partial without enough reason

Observed release readiness rows include:

```text
/kanon/assistance live partial
/kanon/review-queue live partial
/kanon/policies live partial
```

Problem:

- Partial status is useful only if it says what is missing.
- Operators need to know whether partial means empty data, missing route, insufficient role, generated client gap, fixture fallback, or deliberate scope boundary.

Required fix:

- Add reason-bearing partial states to these pages.
- Empty states must tell the operator what to do next.

### KANON-FINDING-006 — Evidence links need contextual labels

Observed domain-pack and agent-interaction surfaces show many generic links:

```text
Decision
Spine
Copy ID
Evidence spine
Open
```

Problem:

- The operator cannot always tell whether a link opens a validate decision, load decision, planning decision, model call, or general Evidence Spine graph.

Required fix:

- Use contextual labels: `Open validate decision`, `Open load decision`, `Open Evidence Spine for pack`, `Copy load decision ID`.
- Use consistent accessible names and copy feedback.

## System-wide issues to surface but not solve in this sprint

These affect Kanon pages but should not expand this sprint into backend/system work:

1. `/health` payload format warnings across services.
2. Missing service version metadata.
3. Trace bridge still “ready to test,” not proven.
4. Conexus readiness strict is not ready.
5. Fixture/demo release readiness is too green.

The Kanon console may display these truths more clearly, but this package does not require backend implementation.
