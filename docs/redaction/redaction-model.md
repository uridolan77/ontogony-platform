# Redaction model

`Ontogony.Redaction` is a mechanical redaction layer for logs, metadata, provider errors, prompts, responses, and secret display.

It is not a privacy classifier and does not decide whether a payload may be stored. It gives services a deterministic way to mask fields known to be sensitive.
