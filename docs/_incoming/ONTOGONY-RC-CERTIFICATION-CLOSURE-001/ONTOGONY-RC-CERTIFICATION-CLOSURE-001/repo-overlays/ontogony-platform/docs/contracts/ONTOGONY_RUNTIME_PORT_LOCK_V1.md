# ONTOGONY_RUNTIME_PORT_LOCK_V1

Canonical local runtime service identity and port lock.

| Service | Port | Required identity |
|---|---:|---|
| Kanon | 5081 | `kanon` |
| Conexus | 5082 | `conexus` |
| Allagma | 5083 | `allagma` |
| Metabole | 5084 | `metabole` |
| Aisthesis | 5085 | `aisthesis` |

## Rules

1. Docker compose files must bind these ports.
2. Live certification scripts must not hardcode conflicting defaults.
3. Producer BaseUrls must be validated against this lock.
4. `/ready` should expose service identity whenever possible.
5. A mismatch is a hard failure, not a warning.
