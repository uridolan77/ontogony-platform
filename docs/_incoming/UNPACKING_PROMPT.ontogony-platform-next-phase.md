# Unpacking prompt

You are in the parent folder that contains `ontogony-platform`.

Overlay zip path:
`ontogony-platform/docs/_incoming_packages/ontogony-platform-next-phase-overlay.zip`

Task:
Apply this planning overlay to `ontogony-platform`.

PowerShell:

```powershell
$zip = "ontogony-platform/docs/_incoming_packages/ontogony-platform-next-phase-overlay.zip"
$temp = ".tmp/ontogony-platform-next-phase-overlay"

if (!(Test-Path "ontogony-platform")) { throw "Run from parent folder containing ontogony-platform." }
if (!(Test-Path $zip)) { throw "Missing overlay zip: $zip" }

Remove-Item $temp -Recurse -Force -ErrorAction SilentlyContinue
New-Item -ItemType Directory -Force -Path $temp | Out-Null
Expand-Archive -Path $zip -DestinationPath $temp -Force

Copy-Item "$temp/ontogony-platform/*" "ontogony-platform" -Recurse -Force

git -C ontogony-platform status --short
```

After unpacking, verify these exist:

- `docs/planning/next-phase/CURRENT_REVIEW_AFTER_PLAT011.md`
- `docs/planning/next-phase/NEXT_PHASE_SEQUENCE.md`
- `docs/planning/next-phase/QUALITY_GATES.md`
- `docs/planning/next-phase/pr-specs/`
- `_agent_prompts/platform/`
- `_issue_bodies/platform/`

Do not implement code during unpacking. This is a planning/spec overlay.
