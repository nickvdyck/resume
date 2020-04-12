<#
 .SYNOPSIS
    Build resume.json file

 .DESCRIPTION
    Validate and build resume.json file

 .PARAMETER artifacts
    Output folder
#>

param(

    [String] $Output = "$PSScriptRoot/../artifacts"

)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

. "$PSScriptRoot/utils.ps1"

Remove-Item -Recurse $Output -ErrorAction Ignore

Exec dotnet tool restore

Exec resume validate -f "src/resume.json"

Exec resume build -f "src/resume.json" -o "$Output"

Copy-Item -Path "./src/resume.json" -Destination "$Output"
