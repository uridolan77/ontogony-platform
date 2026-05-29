# Cursor prompt — producer recheck only

Use this prompt only if Aisthesis RC-readiness reveals producer drift.

For each producer repo:

1. Run full Release build/test with APIs stopped.
2. Verify Aisthesis emission tests still pass.
3. Verify owned required edges still match:
   - Allagma: run → Kanon plan, run → Kanon decision, run → Conexus model call.
   - Kanon: decision → semantic plan, decision → policy.
   - Conexus: model call → route decision, providerAttemptId.
   - Metabole: pipeline → profile, pipeline → mapping, mapping → artifact.
4. Do not change domain behavior unless emitter evidence is wrong.
5. Record exact results in repo evidence docs.
