# Implementation Depth > 9 master plan

## Scoring definition

A repo scores above 9 in implementation depth when:

- its core role is exercised through representative runtime/code paths;
- major partial items are either implemented, explicitly blocked with startup/test gates, or isolated to a precise next-stage backlog;
- durable mode is tested where state matters;
- operator-relevant evidence is emitted and validated;
- contract/runtime consumers have automated proof, not only prose;
- local stack smoke and package-mode proof do not contradict each other.

## Cross-repo success criteria

```text
1. Platform compatibility gate passes.
2. Conexus default CI-equivalent tests pass.
3. Kanon default tests + route/client/openapi/manifest filters pass.
4. Allagma default tests pass.
5. Allagma cross-repo conformance passes.
6. Runtime lock validation passes.
7. Feature connection matrix validates against moving main or new lock.
8. Governed fake E2E passes.
9. Governed fake replay E2E passes.
10. Governed MAF E2E passes.
11. Postgres semantic smoke passes for Kanon.
12. Postgres persistence/replay smoke passes for Allagma where enabled.
13. Conexus persistence smoke passes where enabled.
14. Real external tool execution remains blocked and verified.
```

## Implementation themes

- Platform: deepen mechanics without owning product authority.
- Conexus: deepen gateway operations, provider capability, fallback, persistence, and streaming posture.
- Kanon: deepen semantic authority, lifecycle, source/fact graph, governance, and Postgres proof.
- Allagma: deepen governed runtime, replay orchestration, topology E2E, streaming path, and MAF adapter depth.
