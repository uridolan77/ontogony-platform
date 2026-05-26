# Risks and non-goals

## Risks

1. **Over-centralization risk**
   A single status abstraction can become too generic. Avoid this by keeping dimensions separate.

2. **Truth-softening risk**
   Better wording must not hide failures. If Conexus is reachable but not ready, say that plainly.

3. **Visual-noise risk**
   Too many badges can clutter pages. Use hierarchy: primary outcome, then badges, then details.

4. **Backend-contract drift risk**
   The frontend may be forced to infer states from inconsistent backend shapes. Mark inferred states explicitly.

5. **False-readiness risk**
   Generated/fixture/demo states must not be used for readiness claims.

## Non-goals

- Do not implement the full system compatibility manifest. That belongs to `SYSTEM-TRUTH-001`.
- Do not fix Evidence Spine route resolution. That belongs to `EVIDENCE-SPINE-REAL-001`.
- Do not implement Agent Interaction live replay. That belongs to `ALLAGMA-AGENT-INTERACTION-001`.
- Do not overhaul Kanon source-binding/domain-pack workflows. That belongs to `KANON-CONSOLE-POLISH-001`.
- Do not enable real external execution.
