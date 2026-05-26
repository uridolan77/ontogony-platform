Skill Lab API/Error Semantics + OpenAPI Parity Cleanup
Goal

Make the Skill Lab operator surface production-grade as a read-only inspection UI after 001H makes runs durable.

Current weakness to solve:

The Skill Lab works, but frontend failure modes are still too blunt.
The API client can collapse not-found / unauthorized / unavailable into null.
Skill optimization routes are still temporarily listed as OpenAPI parity gaps.

001I should make the Skill Lab honest and diagnostically useful.

Scope
1. Harden frontend API result semantics

Replace null-only client behavior with typed outcomes.

Current pattern:

if (!result.ok) return null;

Target shape:

export type SkillLabApiResult<T> =
  | { kind: "success"; data: T }
  | { kind: "not-configured" }
  | { kind: "not-found"; message?: string }
  | { kind: "unauthorized"; message?: string }
  | { kind: "unavailable"; message?: string }
  | { kind: "server-error"; message?: string }
  | { kind: "unknown-error"; message?: string };

Apply to:

listSkillOptimizationRuns
getSkillOptimizationRunEvidence

The hooks should preserve a clean UI-facing shape:

{
  data,
  state,
  isLoading,
  isFetching,
  isConfigured,
  error
}
2. Add explicit Skill Lab UI states

The list page should distinguish:

Allagma not configured
Allagma unauthorized
Allagma unavailable
No skill optimization runs
Skill optimization API returned error

The detail page should distinguish:

run not found
unauthorized
backend unavailable
evidence export unavailable

Do not show “run not found” for every failed response.

3. OpenAPI parity cleanup

001G temporarily added skill-optimization routes to frontend parity gaps because the backend snapshot was not yet committed.

001I should remove or narrow this waiver.

Preferred outcome:

1. Regenerate Allagma OpenAPI snapshot after 001H.
2. Sync frontend OpenAPI snapshot.
3. Remove all skill-optimization routes from ALLAGMA_INVENTORY_OPENAPI_GAPS.
4. Update ALLAGMA_SNAPSHOT_PROVENANCE.json.
5. Confirm route parity passes without special waiver.

Routes to cover:

GET  /allagma/v0/skill-optimization/runs
POST /allagma/v0/skill-optimization/runs
GET  /allagma/v0/skill-optimization/runs/{runId}
GET  /allagma/v0/skill-optimization/runs/{runId}/operations
POST /allagma/v0/skill-optimization/runs/{runId}/start
POST /allagma/v0/skill-optimization/runs/{runId}/cancel
GET  /allagma/v0/skill-optimization/runs/{runId}/evidence

If 001H adds repository mode / persistence diagnostics, include those too.

4. Confirm route catalog coverage

Verify both frontend routes are present in:

release-route-catalog.json
coverage-catalog.json
route smoke coverage

Routes:

/allagma/skill-lab
/allagma/skill-lab/:runId

001G already did most of this; 001I should close the loop after OpenAPI parity is fixed.

5. Add frontend tests

Add unit/component tests for:

empty runs list
not configured
unauthorized
backend unavailable
run not found
evidence export server error
limitation badges still visible on happy path

Extend Playwright minimally:

happy path remains green
empty list state
unknown run detail state
backend unavailable state

Do not overdo this; 001I is hardening, not a UI rebuild.

6. Backend/API semantics check

Allagma should return consistent envelopes for:

404 missing run
400 invalid request
401 unauthorized
409 invalid operation transition
500 unexpected error

If the endpoint already returns CrossServiceErrorEnvelope, confirm all skill endpoints follow that convention.

If not, fix them.

7. Evidence doc

Add:

ontogony-frontend/docs/evidence/ONTOGONY_SKILL_OPTIMIZATION_SPINE_001I.md

If backend OpenAPI changed, also add/update:

allagma-dotnet/docs/evidence/ONTOGONY_SKILL_OPTIMIZATION_SPINE_001I.md

Include:

scope
routes verified
OpenAPI parity status
UI states added
tests run
known limitations
explicit non-goals
Non-goals

Do not implement:

real optimizer calls
live deployment
skill injection
operator mutation controls
deployment bindings
new Allagma lifecycle states unless required by 001H persistence
Acceptance criteria

001I is closed when:

1. Skill Lab does not collapse all API failures into null.
2. Empty / not configured / unauthorized / unavailable / not found states are distinct.
3. Skill optimization routes are removed from broad OpenAPI parity gaps, or the waiver is narrowed with evidence.
4. TypeScript passes.
5. Playwright happy path still passes.
6. At least one non-happy-path Skill Lab test passes.
7. No mutation controls are introduced.
Suggested commit title
fix(skill-lab): harden API states and close skill optimization route parity