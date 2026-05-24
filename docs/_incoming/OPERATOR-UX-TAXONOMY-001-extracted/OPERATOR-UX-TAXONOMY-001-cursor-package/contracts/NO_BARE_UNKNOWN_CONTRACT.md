# No bare unknown contract

## Rule

The UI must not display bare `unknown` as a complete operator-facing value.

## Bad

```text
unknown
Status: unknown
Provider: unknown
```

The last example is still insufficient unless it includes the reason when known.

## Good

```text
Provider: unknown — model-call record did not include provider metadata
Compatibility: unknown — no compatibility summary artifact was found
Readiness: unknown — /ready has not been queried
Task type: unknown — task classification event is absent
```

## Implementation helper

Add a helper similar to:

```ts
export function labeledUnknown(subject: string, reason?: string): OperatorStatusViewModel {
  return {
    dimension: 'unknown',
    state: 'unknown',
    label: `${subject}: unknown`,
    reason: reason ?? 'No reason was provided by the source adapter.',
    severity: 'warning'
  };
}
```

## Tests

Tests should scan rendered output for common violations:

- `>unknown<`
- `Status: unknown` without subject-specific context
- table cells containing only `unknown`
- unlabelled unknown in run cards, provider cards, compatibility cards, settings rows
