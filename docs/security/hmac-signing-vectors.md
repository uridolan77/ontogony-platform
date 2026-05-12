# HMAC Signing Vectors

Deterministic vectors for cross-service verification tests.

## Vector 1: JSON body with explicit key-id

Inputs:

- Secret: `unit-test-secret`
- ServiceId: `svc-a`
- KeyId: `k-current`
- Method: `POST`
- PathAndQuery: `/events?x=1`
- Timestamp: `1747045800`
- Nonce: `nonce-123`
- Body bytes (UTF-8): `{}`

Body hash (SHA-256, lowercase hex):

```text
44136fa355b3678a1146ad16f7e8649e94fb4fc21fe77e8310c060f61caaff8a
```

Canonical string:

```text
POST
/events?x=1
1747045800
nonce-123
44136fa355b3678a1146ad16f7e8649e94fb4fc21fe77e8310c060f61caaff8a
```

Expected signature (Base64): computed by
`ServiceIdentityHmacSignatureHelper.ComputeSignatureBase64(secret, method, pathAndQuery, timestamp, nonce, bodyHash)`.

## Vector 2: Empty body request

Inputs:

- Secret: `unit-test-secret`
- Method: `GET`
- PathAndQuery: `/health`
- Timestamp: `1747045800`
- Nonce: `nonce-empty`
- Body bytes: empty

Empty-body hash (SHA-256 lowercase hex):

```text
e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855
```

Canonical string:

```text
GET
/health
1747045800
nonce-empty
e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855
```

## Usage guidance

- Keep vectors in UTF-8.
- Do not trim canonical lines.
- Body hash is over raw body bytes exactly as transmitted.
- Reject requests when key-id is unknown.
