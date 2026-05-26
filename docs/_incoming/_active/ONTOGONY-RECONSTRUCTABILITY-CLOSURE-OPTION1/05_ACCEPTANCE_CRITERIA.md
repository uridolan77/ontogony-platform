# Acceptance criteria

## Package-level closure

The package is closed only when:

```text
PR-001 Allagma → Kanon classifier closure: PASS
PR-002 Conexus emitters: PASS
PR-003 Conexus → Kanon classifier closure: PASS
PR-004 Cross-service golden trace: PASS
PR-005 Platform conformance kits: PASS or explicitly split into a follow-up with no backend closure claims
```

Frontend PR-006 is optional and must not block backend closure.

## Decision-event minimums

For all high/critical events:

```text
inputs: present
policyBasis: present unless explicitly exempt by profile
operatorIdentity or projectIdentity: present
authorizationEnvelope: present
outputAction: present
postConditionState: present and verified if backed by persisted domain state
relatedIds: sufficient to traverse trace/run/model/decision
evidenceFragments: every referenced fragment must resolve
```

## Classification gate

A decision event with severity `high` or `critical` must not classify:

```text
governanceStatus = FAIL
```

Acceptable:

```text
PASS
WARN
```

`WARN` is acceptable only if diagnostics identify non-blocking evidence weakness.

## Redaction gate

No decision-event export may leak:

```text
raw user prompt
raw model completion
raw API key
provider secret
authorization token
connection string
unredacted PII
tool payload with sensitive values
```

Hashes and stable identifiers are allowed.

## Trace identity gate

Every cross-service fixture must preserve:

```text
traceId
correlationId
runId where applicable
decision ids
model call ids
route decision ids
```

## Drift gates

Any new API endpoint must update:

```text
route inventory
OpenAPI baseline where applicable
client coverage where applicable
feature connection matrix
current-state docs
known-limitations docs
tests
```
