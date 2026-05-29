# Cursor prompt — Combined producer recheck

Use only if Aisthesis v2 certification shows missing producer evidence.

For each missing edge diagnostic:

1. Identify `producerOwner`.
2. Go to that repo only.
3. Find the existing Aisthesis emitter/factory/writer.
4. Add or fix emission for the missing relation/native ID.
5. Add unit tests.
6. Re-run Aisthesis live/fixture certification.
7. Do not add cross-repo dependencies that violate boundaries.
