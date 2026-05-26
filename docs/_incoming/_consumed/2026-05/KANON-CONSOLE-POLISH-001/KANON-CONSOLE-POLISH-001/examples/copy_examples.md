# Copy examples

## Assistance

```text
Draft-only model assistance
Kanon can ask Conexus for draft notes, explanations, or classification hints. The result is advisory only and cannot mutate ontology versions, source bindings, domain packs, policies, or gates. Acceptance requires governed human review.
```

```text
Redaction preview
The following fields will be sent: summary, sourceType, changeIntent.
No fields will be force-redacted.
```

## Domain packs

```text
Packs on disk
Domain-pack files discovered by Kanon in the configured pack location.
```

```text
Active ontology versions
Ontology versions currently marked active in Kanon lifecycle state. These may include local/generated versions in development environments.
```

```text
Load disabled — this version is already active.
```

## Review Queue

```text
No review items returned
Kanon returned an empty review queue for the current ontology version and actor role. Try refreshing, changing ontology version, or checking whether review workflows are configured.
```

## Policies

```text
Partial policy surface
Live policy data loaded, but review actions are not exposed by the current client snapshot.
```
