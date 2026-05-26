# Prompt 03 — Kanon Semantic Graph

```text
Implement or adjust the Kanon side needed for EVIDENCE-SPINE-REAL-001.

Tasks:
1. Inspect /ontology/v0/semantic-graph and decision-record/provenance APIs.
2. Identify why semantic graph emits placeholder cross-service nodes, especially Allagma run placeholders.
3. Preserve placeholder emission if useful, but include enough canonical IDs and authority metadata for frontend merge.
4. Ensure decision -> ontology version, provenance, trace, correlation, actor, source binding, policy, and plan edges are explicit where Kanon actually knows them.
5. Do not fabricate Allagma/Conexus details inside Kanon. Kanon may reference external IDs, but authoritative details come from their owning services.
6. Add metadata to distinguish placeholder/reference nodes from authoritative Kanon-owned nodes.
7. Add tests around semantic graph output for a known decision record.

Important:
Kanon owns semantic truth, decisions, provenance, policies, and ontology relationships. It must not become the source of truth for Allagma run details or Conexus provider route details.
```
