# LES-001 / LES-002 proof discipline

## LES-001

Expected role: core live proof of four producers observed with complete reconstructability.

Required fields:

```yaml
traceId:
artifactPath:
producersObserved:
requiredEdges:
reconstructabilityGrade:
score:
blockingFindings:
bundleFingerprint:
accepted:
```

## LES-002

Expected role: Metabole-first or alternate workflow proof.

Known prior state from RC-readiness-005:

```yaml
status: PASS
producersObserved: [allagma, kanon, conexus, metabole]
blockingFindings: 0
grade: partial
scoreApprox: 0.82
```

## Allowed outcomes

### Complete

All applicable required edges are present. Score/grade complete.

### Accepted partial

Allowed only when:

```text
blockingFindings = 0
requiredEdges.applicable.missing = 0 or every missing edge has notApplicableReason
partial dimensions are documented
no compliance-critical evidence missing
future remediation owner exists
```

### Blocked

Any blocking finding, missing mandatory edge without reason, unknown producer coverage, missing bundle fingerprint, or unclear partial reason.
