# Producer token policy

Aisthesis local dev may run with auth `off`. Integration should also work with `producer-token`.

Suggested env vars:

```text
ALLAGMA_AISTHESIS_TOKEN
KANON_AISTHESIS_TOKEN
CONEXUS_AISTHESIS_TOKEN
METABOLE_AISTHESIS_TOKEN
```

Acceptance:

- matching producer token succeeds;
- mismatched body `producerSystem` fails in strict mode;
- no secrets are committed.
