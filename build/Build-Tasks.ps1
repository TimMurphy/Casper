Properties {
    # Following properties are solution specific
    $solutionName = "Casper"
    $mainProject = "Casper.Data.Git"

    # Following properties are generic
    $solutionFolder = (Resolve-Path ..\).ToString()
    $sln = "$solutionFolder\$solutionName.sln"
    $specFlowVersion = "1.9.0"
    $configuration = "Release"
    $packages = "$solutionFolder\packages"
    $nuGet = "$packages\NuGet.exe"
    $nuGetConfig = "$solutionFolder\NuGet.config"
    $nuSpec = "$solutionFolder\build\$solutionName.nuspec"
    $specFlowToolsFolder = "$packages\SpecFlow.$specFlowVersion\tools"
    $specFlow = "$specFlowToolsFolder\specflow.exe"
    $tests = "$solutionFolder\tests"
    $xunit = "$packages\xunit.runners\tools\xunit.console.clr4.exe"
    $artifacts = "$solutionFolder\artifacts"
    $nuGetArtifacts = "$artifacts\NuGet"
    $binArtifacts = "$artifacts\bin"
    $nuGetVersion = Get-NuGet-Version
    $nuPkg = "$solutionName.$nuGetVersion.nupkg"
}

Task default -depends Validate-Properties, Delete-Artifacts, Build

Task Validate-Properties {

    Assert (Test-Path $solutionFolder) "solutionFolder '$solutionFolder' does not exist."
    Assert (Test-Path $sln) "sln '$sln' does not exist."
    Assert (-not [string]::IsNullOrWhitespace($specFlowVersion)) "specFlowVersion is required."
    Assert (-not [string]::IsNullOrWhitespace($configuration)) "configuration is required."
    Assert (Test-Path $packages) "packages '$packages' does not exist."
    Assert (Test-Path $nuGet) "nuGet '$nuGet' does not exist."
    Assert (Test-Path $tests) "tests '$tests' does not exist."

    Write-Host "Successfully validated properties."
    Write-Host
}

Task Build -depends End-to-End-Tests, Create-Bin-Artifacts {

    Create-Folder $nuGetArtifacts
    Create-NuGet-Package -nuGet $nuGet -nuSpec $nuSpec -nuPkg $nuPkg -outputDirectory $nuGetArtifacts -version $nuGetVersion
    MyGet-Cleanup -packages $packages
}

Task Compile -depends Restore-NuGet-Packages, Create-SpecFlow-Tests {

    Compile-Solution $sln $configuration
}

Task Create-Bin-Artifacts {
    
    Create-Bin-Artifacts -bin $solutionFolder\source\$mainProject\bin\$configuration -artifacts $binArtifacts
}

Task Create-SpecFlow-Config {
    
    Create-SpecFlow-Config -specFlowToolsFolder $specFlowToolsFolder
}

Task Create-SpecFlow-Tests -depends Create-SpecFlow-Config {

    Create-SpecFlow-Tests -specFlow $specFlow -testsFolder $tests
}

Task Delete-Artifacts  {

    If (Test-Path $artifacts)
    {
        Remove-Item -Path $artifacts -Recurse -Force
    }        
}

Task End-to-End-Tests -depends Compile {

    Run-xUnit-Tests -xunit $xunit -testsFolder $tests -configuration $configuration
}

Task Restore-NuGet-Packages {

    Restore-NuGet-Packages -nuGet $nuGet -nuGetConfig $nuGetConfig -sln $sln -packages $packages
}

FormatTaskName {
   param($taskName)

   Write-Host $taskName -ForegroundColor Yellow
   Write-Host "----------------------------------------------------------------------" -ForegroundColor Yellow
}
