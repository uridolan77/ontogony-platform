# Patch guide — edge authorization hardening

Preferred low-risk path:

1. Keep existing batch authorization behavior.
2. For direct `POST /evidence/edges`, require wildcard/system token in producer-token mode unless request has authenticated producer identity.
3. Add tests proving non-wildcard producer token cannot create arbitrary direct edge.
4. Keep batch edge-only path available with producerSystem declared.
5. Document relation namespace ownership.

Alternative: add producerSystem to direct edge request contract and authorize relation namespace ownership.
