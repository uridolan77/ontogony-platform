# Copy Guide

## Replace these phrases

### Bad

```text
Kanon trusts X-Ontogony-Actor-* and X-Ontogony-Roles from operator settings (Allagma defaults).
```

### Good

```text
In local operator mode, the console sends actor context headers to Kanon. Kanon authorizes requests according to those roles.
```

---

### Bad

```text
unknown source
```

### Good

```text
source not classified
```

Better: map to one of the defined credential sources.

---

### Bad

```text
Gateway health
```

on Kanon pages.

### Good

```text
Kanon API health
Kanon readiness
```

---

### Bad

```text
No kill switch
```

without severity.

### Good

```text
Kill switch: not configured
Severity: local-alpha warning
```

---

### Bad

```text
apiKey: secret-live-key
```

in sample payloads.

### Good

```json
{
  "summary": "Operator review context.",
  "sourceType": "domain-pack"
}
```

---

### Bad

```text
Actor
```

when context is ambiguous.

### Good

```text
Current operator actor
Historical run actor
Service actor
Request actor
```

## Standard labels

```text
Credential source
Storage scope
Configured
Not configured
Local browser
Session only
Default development value
Environment-injected service secret
Service configured; raw value not visible to console
```

## Risk labels

```text
local-alpha only
safe metadata
sensitive: raw value omitted
warning: browser-local credential
warning: kill switch not configured
blocked: real external execution disabled
```

## Model naming

Use:

```text
Model purpose
Conexus alias
Provider key
Provider model
Resolved route
```

Avoid:

```text
Model
```

when the term could mean any of the above.
