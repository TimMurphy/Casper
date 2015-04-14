@echo off
rem --------------------------------------------------------------------
rem Compiles, tests and creates Casper packages.
rem
rem This script is ideal to run from Windows Explorer as it pauses when
rem script completes. Use .\scripts\build.ps1 for CI servers etc.
rem --------------------------------------------------------------------

pushd %~dp0

powershell .\Build\Build.ps1

popd

echo.
echo.
pause
