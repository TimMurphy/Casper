# --------------------------------------------------------------------
# Compiles, tests and creates Casper packages.
#
# This script is intended for CI servers, etc therefore it will not
# stop for user input or stop to show error messages. If you need 
# that run it from the command line or run ..\build.cmd.
# --------------------------------------------------------------------

$ErrorActionPreference = "Stop"

Push-Location $PSScriptRoot

try
{
    $solutionFolder = (Resolve-Path "$PSScriptRoot\..").ToString()
    $packagesFolder = "$solutionFolder\packages"

    $nuGetFile = "$packagesFolder\NuGet.exe"

    Write-Host
    Import-Module .\Common-Build-Tasks.psm1 -WarningAction Ignore -Force

    Create-Folder $packagesFolder
    Install-NuGet $nuGetFile
    Install-NuGet-Package -solutionFolder $solutionFolder -packageId psake -excludeVersion $true

    Import-Module $packagesFolder\psake\tools\psake.psm1 -Force
    Invoke-psake $PSScriptRoot\Build-Tasks.ps1

    $successful = $psake.build_success
}
Catch
{
    Write-Host $_.Exception.Message -ForegroundColor Red
}
Finally
{
    Write-Host
    Pop-Location

    If ($successful)
    { 
        Write-Host "Build was successful." -ForegroundColor Green
        exit 0
    }
    Else
    {
        Write-Host "Build failed." -ForegroundColor Red
        exit 1
    }
}
