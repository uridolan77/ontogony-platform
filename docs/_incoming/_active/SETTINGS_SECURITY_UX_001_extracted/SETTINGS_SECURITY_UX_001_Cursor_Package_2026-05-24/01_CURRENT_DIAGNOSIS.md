# Current Diagnosis

## Summary

The settings/security UX currently exposes many correct concerns, but in a noisy and ambiguous way. The problem is not primarily missing security machinery; it is that the operator cannot reliably distinguish:

- configured vs unset credentials,
- browser-local values vs environment-provided values,
- current operator actor vs service actor vs historical run actor,
- local trusted-header convenience vs production trust boundary,
- model purpose vs model alias vs provider model,
- redaction policy vs redaction result,
- client diagnostics vs service diagnostics,
- execution blocked vs sandbox disabled vs missing kill switch.

This sprint should make the local-alpha console honest, precise, and easy to operate.

## Raw symptoms to address

### Settings / credentials

- Repeated credential/local-storage warnings dominate the page.
- Secret source sometimes appears as `unknown source`.
- Credential scope is not consistently explained.
- The operator cannot quickly tell if a value is stored in local browser settings, session memory, default/dev config, or injected through environment/service configuration.
- Diagnostics export includes browser data, but the UI should label it as client diagnostics and privacy-reviewable.

### Actor roles

- Kanon pages show current roles sent as `Admin`, while help text says Docker-local expects `Auditor, ProvenanceReader`.
- This is not a code failure, but a product-language contradiction.
- Pages say “Allagma defaults” when the actor context is really coming from operator settings.
- “Kanon trusts headers” is misleading; it sounds like arbitrary header spoofing is an intended security model.

### Redaction / assistance

- Conexus Assistance sample context contains secret-looking values such as `apiKey`.
- Allowed-fields examples include secret-like field names while force-redaction also includes those names.
- Redaction is described, but the operator needs a preview before sending anything to Conexus.

### Actor provenance in evidence views

- Current operator actor can be `local-operator`.
- Historical run/service actor can be `env-seed-001-agent`.
- These may both be correct, but the UI needs labels:
  - Current operator actor
  - Request actor
  - Historical run actor
  - Service actor

### Execution posture

- Runtime posture repeats across Home/Allagma/Settings.
- `No kill switch` needs explicit severity/scope: local-alpha warning vs production blocker.
- Real external execution remains blocked, which is correct, but the setting needs to be understandable without panic.

### Naming precision

- `gpt-4o-mini` appears as a routing default in some settings contexts.
- If it is a Conexus alias, call it `model alias`.
- If it is a concrete provider model, do not present it as an operator-level routing key.
- Prefer purpose/alias language:
  - model purpose: `summarize-player-risk`
  - Conexus alias: `risk-summary-v0`
  - provider key/model: resolved by Conexus.

## Desired operator impression

After this sprint, Settings should answer:

1. What services am I connected to?
2. Which credentials are configured, and where are they stored?
3. Which actor and roles am I currently sending?
4. What local role preset am I using?
5. What capabilities are granted/denied?
6. What will be redacted before AI assistance?
7. What does diagnostics export contain?
8. Why is real execution blocked?
9. What model purpose/alias will be used?
