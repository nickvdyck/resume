<#
 .SYNOPSIS
    Deploys a blob to Azure

 .DESCRIPTION
    Deploys artifacts to an Azure Blob Storage Account

 .PARAMETER artifacts
    Folder to upload to azure
#>

param(

    [String]
    $Output = "$PSScriptRoot/../artifacts"

)

Set-StrictMode -Version 2
$ErrorActionPreference = 'Stop'

. "$PSScriptRoot/utils.ps1"

Remove-Item -Recurse $Output -ErrorAction Ignore

Exec dotnet run `
        -p "src/Resume" `
        -- validate

Exec dotnet run `
        -p "src/Resume" `
        -- build -o "$Output"

Copy-Item -Path "./resume.json" -Destination "$Output"
