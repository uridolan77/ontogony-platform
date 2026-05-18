# Security and Redaction Checklist

- [ ] Raw marker content hidden.
- [ ] Raw tool args hidden.
- [ ] Raw prompts hidden unless an existing safe view supports them.
- [ ] Absolute sandbox root redacted or visually de-emphasized.
- [ ] Relative path allowed.
- [ ] Effect fingerprint allowed.
- [ ] Executor ref allowed.
- [ ] Failure message reviewed for operator safety.
- [ ] External refs rendered with redaction pass.
- [ ] No secrets, tokens, provider headers, API keys, environment variables.
