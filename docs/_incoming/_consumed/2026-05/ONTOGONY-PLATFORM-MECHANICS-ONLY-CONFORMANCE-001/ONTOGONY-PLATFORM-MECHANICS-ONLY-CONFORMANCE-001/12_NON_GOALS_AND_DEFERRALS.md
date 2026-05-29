# Non-goals and deferrals

## Non-goals

- Production readiness.
- Runtime lock authority.
- Semantic authority.
- Model routing.
- Governed execution.
- SLOD mapping acceptance.
- Reconstructability scoring.
- Replay runtime.

## Likely deferrals

| Deferral | Owner | Reason |
|---|---|---|
| Live service conformance mode | Product repos | Needs running local services |
| NuGet package-mode full validation for all consumers | Platform + consumers | Requires feed/versioning policy |
| Replay runtime | Product repos | Platform owns contracts only |
| Numeric coverage thresholds | Platform | Existing posture keeps coverage advisory |
| Branch protection | Repo admin | Not implemented by this package |
