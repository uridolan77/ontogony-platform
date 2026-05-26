# Risks and Non-Goals

## Risks

### Overbuilding

A full skill-training platform is too large for one slice. Build a deterministic vertical slice first.

### Console clutter

Skill optimization creates many objects. The frontend must summarize and collapse detail rather than dumping raw records.

### Silent behavioral drift

If skill versions can change without held-out gates and deployment bindings, the system becomes less governable. Enforce immutable versions and explicit deployment.

### Benchmark theater

Do not add meaningless fake scores. Fixture scores should prove lifecycle behavior. Real benchmark support can come later.

### Prompt injection / unsafe skill content

Skills are executable influence. Validation must reject obvious unsafe instructions, scope violations, and policy contradictions.

### Confusing skills with policies

A skill can reference policy but should not replace Kanon authority. Policy remains governed separately.

## Non-goals for first slice

- No model weight training.
- No autonomous production self-modification.
- No marketplace/library of hundreds of skills.
- No statistical evaluation framework beyond deterministic fixture comparison.
- No hidden chain-of-thought capture.
- No mandatory live OpenAI dependency.
- No large new persistence architecture unless already aligned with repo patterns.
