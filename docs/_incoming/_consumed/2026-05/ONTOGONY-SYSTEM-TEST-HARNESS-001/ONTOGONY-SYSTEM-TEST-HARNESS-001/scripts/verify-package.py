from pathlib import Path
import json

root = Path(__file__).resolve().parents[1]
required = [
    'README.md',
    'docs/00_EXECUTIVE_TEST_STRATEGY.md',
    'manifests/services.yml',
    'manifests/routes.yml',
    'manifests/test-catalog.yml',
    'tests/dotnet/Ontogony.SystemTests/Ontogony.SystemTests.csproj',
    'tests/ui-playwright/package.json',
    'load/k6/system-smoke.js',
]
missing = [p for p in required if not (root / p).exists()]
if missing:
    raise SystemExit('Missing required files: ' + ', '.join(missing))
print(json.dumps({
    'package': 'ONTOGONY-SYSTEM-TEST-HARNESS-001',
    'files': len([p for p in root.rglob('*') if p.is_file()]),
    'status': 'ok'
}, indent=2))
