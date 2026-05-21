# Troubleshooting

## Pinned commit checkout fails

Check:

```powershell
git ls-remote https://github.com/uridolan77/<repo>.git <sha>
```

For private repos, verify token access.

## Package-mode restore uses sibling source

Search build logs for:

```text
ProjectReference
../kanon-dotnet
../conexus-dotnet
../ontogony-platform
```

Package-mode release must fail if these appear in restore assets.

## System cohesion fails before scenarios

Likely stack startup, ports, service token, or Postgres.

Check:

```text
Kanon     http://localhost:5081/health
Conexus   http://localhost:5082/health
Allagma   http://localhost:5083/health
```

## Conexus capacity is inconclusive

Capacity baseline should be inconclusive rather than fail if:

- capacity env gate not set;
- Postgres optional scenario requested without connection string;
- capacity report missing due to test host startup failure.

In release mode, required capacity report must exist unless explicitly waived in the release PR.

## Streaming smoke fails

Check Allagma model purpose config:

```json
"Stream": true,
"PersistStreamedOutput": false
```

Check that Conexus rejects streaming idempotency by design and that Allagma omits idempotency headers for streaming.
