#!/usr/bin/env bash
set -euo pipefail

echo "Running ONTOGONY-SYSTEM-TEST-HARNESS-001 .NET system tests..."
cd "$(dirname "$0")/../tests/dotnet"
dotnet test ./Ontogony.SystemTestHarness.sln --logger "trx;LogFileName=ontogony-system-tests.trx"
