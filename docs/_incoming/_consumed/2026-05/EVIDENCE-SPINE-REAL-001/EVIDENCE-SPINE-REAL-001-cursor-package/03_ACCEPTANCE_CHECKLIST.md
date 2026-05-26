# 03 — Acceptance Checklist

## Core resolver behavior

- [ ] Evidence Spine has a stable missing-reason enum or equivalent typed reason model.
- [ ] Every unresolved expected relationship has `applicability`, `reasonCode`, `message`, and `suggestedNextStep`.
- [ ] No source attempt renders only `An unexpected error occurred`.
- [ ] Generic caught exceptions are mapped to structured failure codes.
- [ ] Direct Conexus model-call root does not hard-require a Kanon decision.
- [ ] Governed Allagma run root does require Kanon planning decision and Conexus model call.
- [ ] Trace/correlation root uses contextual applicability rather than hard-coded universal expectations.

## Graph quality

- [ ] Placeholder node and authoritative node for same canonical ID merge into one graph node.
- [ ] Merged nodes preserve all source attempts and page links.
- [ ] Graph node display identifies placeholder-derived metadata without duplicating the entity.
- [ ] Partial graph summary lists missing relationships by reason code.
- [ ] Export bundle includes missing reasons and source attempts.

## Route decision evidence

- [ ] Evidence links that expose `routeDecisionId` are followed.
- [ ] `/admin/v0/route-decisions/{routeDecisionId}` returns detail for fake-provider governed run, or returns a typed 404/error that the resolver maps correctly.
- [ ] Route-decision node includes alias, selected provider, selected provider model, fallback use, route status, and source endpoint when available.
- [ ] Missing route detail is reported as `backend_missing`, `not_recorded`, or `lookup_failed`, not generic error.

## ID clarity

- [ ] Model call ID is displayed separately from request ID.
- [ ] Execution run ID is displayed separately from model call ID.
- [ ] Route decision ID is displayed separately from request ID.
- [ ] Copy buttons copy the specific displayed ID.

## Allagma route normalization

- [ ] No API source attempt displays `/runs/{runId}` as if it were a backend route.
- [ ] All Allagma API route attempts use `/allagma/v0/...` templates.
- [ ] Frontend page links are explicitly labeled as page links, not API source attempts.

## Governed fake-provider proof

- [ ] Start a local governed run through Allagma.
- [ ] Resolve by Allagma run ID.
- [ ] Resolve by Kanon planning decision ID.
- [ ] Resolve by Conexus model call ID.
- [ ] Resolve by route decision ID if recorded.
- [ ] Export evidence bundle.
- [ ] Bundle contains no duplicate canonical nodes.
- [ ] Bundle contains no generic source failure messages.
- [ ] Required governed chain is visible: Allagma -> Kanon -> Conexus -> provider attempt.

## Tests

- [ ] Unit tests cover applicability matrix.
- [ ] Unit tests cover missing reason mapping.
- [ ] Unit tests cover graph canonicalization/merge.
- [ ] Unit tests cover ID normalization.
- [ ] Integration/contract test covers route decision detail resolution or typed structured failure.
- [ ] E2E/manual script covers governed fake-provider chain.

## Documentation

- [ ] Evidence Spine docs explain `not_applicable` vs `missing`.
- [ ] Source attempt docs explain status and reason codes.
- [ ] Operator-facing copy avoids fixture/demo completeness claims.
