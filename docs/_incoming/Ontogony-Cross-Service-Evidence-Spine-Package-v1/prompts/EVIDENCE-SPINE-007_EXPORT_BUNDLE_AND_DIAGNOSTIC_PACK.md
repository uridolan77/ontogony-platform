# EVIDENCE-SPINE-007 — export bundle and diagnostic pack

Goal:
Create a redacted export bundle for the resolved evidence spine graph.

Repos:
- C:\dev\ontogony-frontend
- C:\dev\ontogony-platform for schema/evidence
- backend repos only if backend bundle route is chosen

Tasks:

1. Define schema:
   ontogony-platform/docs/schemas/ontogony-cross-service-evidence-spine-bundle-v1.schema.json

2. Frontend export builder:
   - graph
   - source attempts
   - warnings
   - page links
   - build metadata
   - redaction metadata

3. UI:
   - Copy JSON
   - Download JSON
   - Include/exclude raw previews toggle if appropriate
   - show schema/version

4. Tests:
   - schema fixture validates
   - export includes graph nodes
   - export excludes sensitive fields
   - warnings included

5. Evidence:
   - ontogony-frontend/docs/evidence/EVIDENCE_SPINE_007_EXPORT_BUNDLE_EVIDENCE.md
   - ontogony-platform/docs/evidence/EVIDENCE_SPINE_007_EXPORT_BUNDLE_EVIDENCE.md

Acceptance:
- operator can export a single cross-service evidence bundle from the workbench
- bundle validates against schema
