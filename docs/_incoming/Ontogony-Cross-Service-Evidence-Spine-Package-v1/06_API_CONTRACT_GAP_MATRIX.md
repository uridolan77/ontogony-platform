# API contract gap matrix

| Evidence need | Current likely source | Gap risk | Action |
|---|---|---|---|
| Run by runId | Allagma `GET /runs/{runId}` | Low | Use existing |
| Run events | Allagma `GET /runs/{runId}/events` | Low | Use existing |
| Run audit bundle | Allagma `GET /runs/{runId}/audit` | Low | Use existing |
| Runs by traceId | Allagma `GET /runs?traceId=` | Low/medium | Verify filter behavior |
| Eval by evalId | Allagma `GET /evaluations/{id}` | Low | Use existing |
| Eval evidence export | Allagma `GET /evaluations/{id}/evidence` | Low | Use existing |
| Eval list by runId | Allagma `GET /runs/{runId}/evaluations` | Low | Use existing |
| Baseline comparison by id | Allagma `GET /evaluations/baseline-comparisons/{id}` | Low | Use existing |
| Model call/request detail | Conexus admin/read endpoint | Medium | Audit exact route/status |
| Route decision by id | Conexus `GET /admin/v0/route-decisions/{id}` | Low/medium | Verify client wrapper |
| Recent model calls | Conexus | Medium/high | May require CONEXUS-DEEPEN-001 |
| Kanon decision by id | Kanon decision endpoint | Low/medium | Verify wrapper |
| Kanon decision by trace | Kanon trace lookup | Low/medium | Existing frontend resolver uses it |
| Human gate by id | Allagma events | Medium | May need better direct endpoint |
| Unified export bundle | None | High | Design in EVIDENCE-SPINE-007 |
