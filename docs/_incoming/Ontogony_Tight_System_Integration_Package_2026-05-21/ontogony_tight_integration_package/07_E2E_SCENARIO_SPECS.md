# 07 — E2E scenario specs

## Scenario A — Completed governed run

**Purpose:** prove Allagma → Kanon → Conexus core loop.

### Steps

1. Start local stack.
2. POST `/allagma/v0/runs` with model purpose `summarize-player-risk`.
3. Confirm run reaches `Completed`.
4. Fetch run events.
5. Fetch run audit bundle.

### Required evidence

- Allagma run id.
- Kanon planning decision id.
- Conexus model call id.
- route decision id where available.
- trace/correlation id.

## Scenario B — Idempotent run start

**Purpose:** prove retry safety at runtime boundary.

### Steps

1. POST same run with same `Idempotency-Key` and same payload.
2. Confirm same result or safe replay semantics.
3. POST same key with different payload.
4. Confirm conflict with recovery guidance.

### Required evidence

- idempotency key.
- original run id.
- duplicate response.
- conflict response.

## Scenario C — Human gate wait/approve/deny

**Purpose:** prove Kanon governs consequential actions and Allagma resumes safely.

### Steps

1. Start run that triggers human gate.
2. Confirm pause/waiting state.
3. Resolve approval in Kanon or through Allagma resume path.
4. Confirm run completes.
5. Repeat denial path.

### Required evidence

- human gate id.
- Kanon gate check decision.
- resolve decision.
- Allagma run state transition.
- semantic graph by humanGateId.

## Scenario D — Kanon Conexus assistance

**Purpose:** prove model assistance remains non-authoritative.

### Steps

1. Enable Kanon Conexus assistance in local stack.
2. Call assistance route through cohesion smoke.
3. Confirm `draft_only` decision record.
4. Accept/reject review where configured.
5. Fetch Kanon provenance and semantic graph.

### Required evidence

- assistance decision id.
- model invocation id.
- review decision id when present.
- Kanon `draft_only` status.

## Scenario E — Conexus fallback

**Purpose:** prove provider fallback is observable through Allagma-initiated run.

### Steps

1. Configure fake primary retryable failure + fallback success.
2. Start Allagma run.
3. Confirm completion.
4. Fetch Conexus route decision/model-call evidence.

### Required evidence

- fallback provider attempt chain.
- route decision id.
- model call id.
- usage/cost or quota context if present.

## Scenario F — Streaming lifecycle

**Purpose:** prove opt-in streaming path without payload retention.

### Steps

1. Use model purpose with `Stream=true`, `PersistStreamedOutput=false`.
2. Start run.
3. Confirm stream lifecycle events:
   - started,
   - chunk received count > 0,
   - completed or interrupted.
4. Fetch audit bundle and operator panel data.

### Required evidence

- stream purpose.
- alias used.
- chunk count.
- completed/interrupted status.
- proof no payload persisted by default.

## Scenario G — Restart survival

**Purpose:** prove durable runtime state.

### Steps

1. Run with Postgres persistence.
2. Stop/restart Allagma container/process.
3. Fetch prior run/events/audit after restart.
4. Resume if run was waiting.

### Required evidence

- database connection mode.
- pre-restart run state.
- post-restart readback.
- final state.

## Scenario H — Operator audit journey

**Purpose:** prove a human operator can navigate the system.

### Steps

1. Start from run id.
2. Open audit view.
3. Navigate to Kanon decision/provenance.
4. Navigate to Conexus model-call evidence.
5. Display unresolved edges if any service evidence is absent.

### Required evidence

- screenshots or Playwright JSON.
- ids traversed.
- route calls made.
- unresolved edge count.
