#!/usr/bin/env bash
set -euo pipefail

ROOT="${1:-.}"

echo "== OPERATOR-UX-TAXONOMY-001 validation scan =="
echo "Root: $ROOT"

fail=0

scan_for() {
  local label="$1"
  local pattern="$2"
  echo "-- scanning: $label"
  if grep -RIn --exclude-dir=node_modules --exclude-dir=.git --exclude='*.zip' --exclude='package-lock.json' "$pattern" "$ROOT"; then
    echo "FOUND: $label"
    fail=1
  else
    echo "ok: $label"
  fi
}

# These are strong indicators. Some may be valid in docs/tests, but source/page appearances should be reviewed.
scan_for "page-headline fixture fallback" "Live with fixture fallback"
scan_for "ambiguous failed runs sample" "Failed runs (sample)"
scan_for "developer placeholder copy" "Backend-waiting list APIs"
scan_for "raw leaked parameter error" "The parameter already belongs to a collection"
scan_for "generic unexpected error copy" "An unexpected error occurred"

# Bare unknown is too broad for grep to fail automatically; report candidates.
echo "-- candidate bare unknown occurrences"
grep -RIn --exclude-dir=node_modules --exclude-dir=.git --exclude='*.zip' -E ">unknown<|\bunknown\b" "$ROOT" || true

echo "-- recommended repo commands"
echo "npm test"
echo "npm run lint"
echo "npm run build"

if [ "$fail" -ne 0 ]; then
  echo "Validation found forbidden high-confidence strings. Review and migrate them."
  exit 1
fi

echo "Validation scan completed. Review bare unknown candidates manually."
