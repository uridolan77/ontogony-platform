# Allagma Sampling Events Stub

Add event contracts using existing Allagma event/timeline conventions.

Suggested event names:

```text
SamplingPolicyResolved
SamplingPolicyWarningRaised
SamplingPolicyViolationRaised
SamplingProfileOverrideRequested
SamplingProfileOverrideApproved
SamplingProfileOverrideRejected
```

Minimum fields:

```json
{
  "runId": "string",
  "stepId": "string|null",
  "resolutionId": "string",
  "requestedProfileId": "string|null",
  "effectiveProfileId": "string",
  "decision": "Allowed|AllowedWithWarnings|Denied|RequiresApproval",
  "warningCodes": ["string"],
  "violationCodes": ["string"],
  "createdAtUtc": "datetime"
}
```
