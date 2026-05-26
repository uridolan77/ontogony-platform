# Decision Event Minimums v1

## Canonical properties

A normalized decision event should expose the following properties when applicable:

```text
inputs
policyBasis
operatorIdentity or projectIdentity
authorizationEnvelope
reasoningEvidence
outputAction
postConditionState
relatedIds
evidenceFragments
```

## Required by severity

### Critical

Required:

```text
inputs >= P
policyBasis >= P
operatorIdentity/projectIdentity >= P
authorizationEnvelope >= P
outputAction >= F
postConditionState >= F if persisted domain state exists
evidenceFragments complete
```

No critical event may classify FAIL.

### High

Required:

```text
inputs >= P
policyBasis >= P or explicit profile exemption
operatorIdentity/projectIdentity >= P
authorizationEnvelope >= P
outputAction >= P
postConditionState >= P
evidenceFragments complete
```

No high event may classify FAIL.

### Medium

Required:

```text
inputs present when meaningful
outputAction present
relatedIds present
evidenceFragments complete for any referenced refs
```

WARN acceptable.

### Low

Required:

```text
schema valid
event kind valid
relatedIds sufficient for navigation
```

## Reasoning evidence rule

Do not require hidden chain-of-thought.

Allowed reasoning evidence:

```text
policy summary
routing summary
classifier diagnostics
Kanon decision ID
route decision ID
input/output hash comparison
operator-selected reason code
```

Forbidden:

```text
model hidden reasoning
private chain-of-thought
unredacted prompt/completion
```
