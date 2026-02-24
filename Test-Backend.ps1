#!/usr/bin/env pwsh

<#
.SYNOPSIS
    Orchestrates backend testing and coverage reporting for Echoes of Bathala.

.EXAMPLE
#>

param (
    [Parameter(HelpMessage="Fuzzy match for a project name (e.g., 'api', 'infra')")]
    [string]$Project,

    [Parameter(HelpMessage="Match a specific class or method (e.g., 'TradeTest' or 'ProcessTrade')")]
    [string]$Filter,

    [Switch]$All,
    [Switch]$Report,
    [Switch]$Clean
)

$RESULTS_DIR = "TestResults"
$COVERAGE_DIR = "$RESULTS_DIR/Coverage"
$TESTS_BASE_PATH = Join-Path $PSScriptRoot "backend/tests"

function Show-Header {
    Write-Host "`n---  Echoes of Bathala: Test Runner  ---" -ForegroundColor Cyan
}

if ($Clean) {
    Write-Host "Cleaning old test results..." -ForegroundColor Yellow
    if (Test-Path $RESULTS_DIR) {
        Remove-Item -Recurse -Force $RESULTS_DIR 
    }
    exit
}

Show-Header

$dotnetArgs = @("test", "--verbosity", "minimal", "--logger", "console;verbosity=normal")

if ($Project) {
    $searchPattern = "*GameBackend.*$Project*.Tests.csproj"
    if (-not (Test-Path $TESTS_BASE_PATH)) {
        Write-Error "Test directory not found at $TESTS_BASE_PATH. Are you running this from the root?"
        exit 1
    }

    $projPath = Get-ChildItem -Path $TESTS_BASE_PATH -Recurse -Filter $searchPattern | Select-Object -ExpandProperty FullName -First 1
    if (-not $projPath) {
        Write-Error "Could not find any test project matching '$Project' in $TESTS_BASE_PATH"
        exit 1
    }
    Write-Host "Found Project: $projPath" -ForegroundColor DarkGray
    $dotnetArgs += $projPath
}

if ($Filter) {
    $dotnetArgs += "--filter"
    $dotnetArgs += "FullyQualifiedName~$Filter"
}

if ($Report) {
    $dotnetArgs += "--collect"
    $dotnetArgs += "XPlat Code Coverage"
    $dotnetArgs += "--results-directory"
    $dotnetArgs += $RESULTS_DIR
}

Write-Host "Running: dotnet $($dotnetArgs -join ' ')" -ForegroundColor Gray

& dotnet $dotnetArgs

if ($Report -and $LASTEXITCODE -eq 0) {
    Write-Host "`nGenerating Coverage Report..." -ForegroundColor Magenta

    Write-Host "Restoring local dotnet tools..." -ForegroundColor DarkGray
    & dotnet tool restore

    $coverageFile = Get-ChildItem -Path $RESULTS_DIR -Filter "coverage.cobertura.xml" -Recurse | Select-Object -ExpandProperty FullName -First 1

    if ($coverageFile) {
        & dotnet reportgenerator "-reports:$coverageFile" "-targetdir:$COVERAGE_DIR" "-reporttypes:Html"

        $reportPath = "$COVERAGE_DIR/index.html"

        if (Test-Path $reportPath) {
            Write-Host "Report ready at: $reportPath" -ForegroundColor Green

            if ($IsWindows) {
                Start-Process $reportPath
            } elseif ($IsLinux) {
                xdg-open $reportPath
            }
        } else {
            Write-Warning "Coverage report failed to generate at $reportPath."
        }

    } else {
        Write-Warning "No coverage file found. Ensure coverlet.collector is installed in your test projects."
    }
} elseif ($Report -and $LASTEXITCODE -ne 0) {
    Write-Host "`nTests failed. Skipping coverage report generation." -ForegroundColor Red
}
