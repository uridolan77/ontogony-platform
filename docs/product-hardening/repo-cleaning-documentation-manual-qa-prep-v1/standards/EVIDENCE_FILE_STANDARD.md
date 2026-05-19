# Evidence File Standard v1

```markdown
# <ITEM> — Evidence

**Date:** YYYY-MM-DD  
**Repo(s):** ...  
**Status:** PASS / PARTIAL / FAIL  
**Boundary:** ...

## Scope
...

## Commands
```bash
...
```

## Results
| Check | Result |
| --- | --- |
| ... | PASS |

## Files changed
- ...

## Known limitations
- ...

## Safety
- No secrets
- Not production readiness
```

Evidence must not include API keys, tokens, raw provider request/response bodies, passwords, or unredacted connection strings.
