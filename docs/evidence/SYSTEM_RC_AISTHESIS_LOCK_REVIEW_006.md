# AISTHESIS-LIVE-SPINE-RC-CERTIFICATION-006 — platform lock review

Date: 2026-05-29  
Scope: system-level RC evidence for Aisthesis certification package 006

## System posture

| Component | 006 status |
|---|---|
| Aisthesis evidence spine | RC-certification **partial** |
| Allagma producer alignment | Not re-run in 006 (no live regression signal) |
| Kanon producer alignment | Not re-run in 006 |
| Conexus producer alignment | Not re-run in 006 |
| Metabole producer alignment | Not re-run in 006 |
| ontogony-frontend live spine | HANDOFF_ONLY |
| Production IAM | DEFERRED |
| Retention/erasure | DEFERRED |
| OTel export | DEFERRED |

## Cross-repo certification matrix

See `docs/system/ONTOGONY_FIVE_SERVICE_EVIDENCE_CERTIFICATION_MATRIX.md` for the five-service evidence ownership map.

## Lock recommendation

```yaml
systemRcLockRecommended: false
aisthesisRcCertificationClassification: RC-certification partial
nextGate: live-five-service-PASS
```

## Linked evidence

- Aisthesis closeout: `aisthesis-dotnet/docs/evidence/AISTHESIS_LIVE_SPINE_RC_CERTIFICATION_006_CLOSEOUT.md`
- Live summary (NOT_RUN): `aisthesis-dotnet/artifacts/aisthesis-rc-certification/20260528T224633Z/live-or-explain/summary.json`
- Package source: `ontogony-platform/docs/_incoming/packages/AISTHESIS-LIVE-SPINE-RC-CERTIFICATION-006/`

## Rationale

Aisthesis now exposes v2 required-edge diagnostics, durable in-memory evaluation jobs, hardened edge auth, and honest certification scripts. System RC lock requires live producer evidence across all five services and production gate completion — neither is proven in this session.
