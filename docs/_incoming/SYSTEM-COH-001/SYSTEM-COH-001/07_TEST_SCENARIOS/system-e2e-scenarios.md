# SYSTEM-COH-001 E2E scenario catalogue

## 1. Completed governed run

Flow:

```text
Client → Allagma /runs → Kanon plan/action → Conexus completion → Allagma completed run/events
```

Assertions:

- response has `runId`;
- response has planning decision id where expected;
- response has model call id where expected;
- Allagma events include planning/model/completion events;
- Allagma audit downstream references include Kanon/Conexus locators.

## 2. Idempotent retry

Flow:

- same request with same `Idempotency-Key`;
- verify replay/same response;
- verify no duplicate model call/decision side effect if the system contract promises that.

## 3. Human gate pause/resume

Flow:

- run triggers consequential gate;
- Allagma pauses;
- Kanon gate is resolved;
- Allagma resumes;
- events and decisions remain linked.

## 4. Kanon → Conexus assistance

Flow:

- enable assistance config;
- call assistance route;
- verify redaction / allowed fields;
- verify Conexus model call;
- verify Kanon `draft_only` decision record;
- verify provenance/evidence lookup.

## 5. Conexus fallback

Flow:

- configure fake primary failure and fallback success;
- call through Allagma or direct Conexus smoke depending current support;
- verify fallback chain evidence.

## 6. Correlation chain

Flow:

- send distinct trace and correlation ids;
- verify Allagma events;
- verify Kanon decision record / by-trace;
- verify Conexus execution journal metadata;
- verify Evidence Spine can resolve graph.

## 7. Restart/replay survival

Flow:

- start durable run;
- restart Allagma;
- inspect/resume/replay;
- verify event continuity.

## 8. Real tools blocked

Flow:

- attempt or inspect a real tool execution configuration;
- verify only simulation/sandbox allowed;
- verify no external side-effect execution path is enabled.
