param([string]$AllagmaRoot="C:\dev\allagma-dotnet",[switch]$StartServices,[switch]$SkipFrontendChecks)
$ErrorActionPreference="Stop"; Set-Location $AllagmaRoot
Write-Host "Running guided main flow. This is first working environment, not production readiness."
$args=@(); if($StartServices){$args+="-StartServices"}; if($SkipFrontendChecks){$args+="-SkipFrontendChecks"}
& .\scripts\run-full-sanity.ps1 @args
& .\scripts\validate-full-sanity-report.ps1
Write-Host "Guided main flow PASS."
