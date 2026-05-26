# Backend follow-ups if discovered during frontend work

Do not implement these as part of KANON-CONSOLE-POLISH-001 unless they are already trivial and explicitly within current frontend contracts. Record them as follow-ups instead.

## KANON-BACKEND-FOLLOWUP-001 — Health schema metadata

If Kanon `/health` is still returning non-JSON or non-contract payloads, create a backend follow-up to standardize:

```json
{
  "service": "kanon",
  "status": "healthy",
  "version": "...",
  "gitSha": "...",
  "buildTime": "...",
  "schemaVersion": "health.v1"
}
```

## KANON-BACKEND-FOLLOWUP-002 — Domain-pack version classification

If the frontend cannot reliably identify generated/test-looking versions, request backend metadata:

```json
{
  "packId": "...",
  "version": "...",
  "source": "disk|persisted|generated|test|unknown",
  "isGenerated": false,
  "isTestArtifact": false
}
```

## KANON-BACKEND-FOLLOWUP-003 — Action availability contract

If disabled lifecycle reasons are inferred in UI today, request explicit backend action availability:

```json
{
  "actions": {
    "validate": { "enabled": true, "reason": null },
    "load": { "enabled": false, "reason": "already_active" },
    "promote": { "enabled": false, "reason": "already_accepted_or_active" },
    "deprecate": { "enabled": false, "reason": "cannot_deprecate_active_version" }
  }
}
```

## KANON-BACKEND-FOLLOWUP-004 — Assistance redaction preview endpoint

If redaction preview cannot be exactly reproduced client-side, request a dry-run/preview mode from Kanon assistance routes.

## KANON-BACKEND-FOLLOWUP-005 — Review Queue / Policies partial reasons

If current APIs return no reason codes for partial states, request explicit reason fields for unavailable/empty states.
