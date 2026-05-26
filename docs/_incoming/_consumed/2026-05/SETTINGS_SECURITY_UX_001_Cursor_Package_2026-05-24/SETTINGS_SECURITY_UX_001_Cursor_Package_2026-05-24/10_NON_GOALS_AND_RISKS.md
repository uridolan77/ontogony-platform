# Non-goals and Risks

## Non-goals

- Production authentication or IAM.
- Enterprise RBAC.
- OIDC implementation changes.
- Secret vault implementation.
- Real external execution enablement.
- Sandbox trust-gate implementation.
- Provider key management beyond display/posture clarity.
- Backend rewrite of authorization.
- Complete Evidence Spine work; actor labels only.

## Risks

### Risk: Frontend estimates capabilities incorrectly

Mitigation:
- Label estimated capability summaries as `estimated from local roles`.
- Prefer backend capability introspection later.

### Risk: Copy changes break tests that assert old strings

Mitigation:
- Update tests to assert product semantics, not brittle old wording.
- Add forbidden-phrase tests.

### Risk: Redaction preview gives false confidence

Mitigation:
- Say “redaction preview” not “guaranteed safe.”
- Keep force-redaction conservative.
- Do not include raw values in logs/diagnostics.

### Risk: Role presets overwrite user custom settings

Mitigation:
- Applying a preset must be explicit.
- Custom changes should mark profile as Custom.

### Risk: Diagnostics privacy metadata omits a field

Mitigation:
- Add tests for raw secret absence.
- Keep list of client diagnostic fields explicit.

### Risk: Scope creep into production security

Mitigation:
- Keep local-alpha wording.
- Defer real trust-boundary security to separate security hardening packages.
