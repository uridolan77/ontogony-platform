# FULL-RC-CERTIFICATION-RERUN

## Goal

After all fixes land, run the complete cross-repo confidence suite.

## Required sequence

1. Stop running services.
2. Validate runtime service identity.
3. Run all repo tests.
4. Run package-mode gates.
5. Start canonical five-service stack.
6. Run five-service live certification.
7. Run system cohesion acceptance.
8. Write closeout evidence.

## PASS criteria

The system can be classified as RC-candidate only if all repo-local required tests pass, package-mode gates pass, five-service live cert passes, system cohesion acceptance passes, and no production readiness claim is made.
