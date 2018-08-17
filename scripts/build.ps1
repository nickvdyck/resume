<#
 .SYNOPSIS
    Deploys a blob to Azure

 .DESCRIPTION
    Deploys artifacts to an Azure Blob Storage Account

 .PARAMETER artifacts
    Folder to upload to azure
#>

param(
    [string]
    $Output = "./output"
)

Set-StrictMode -Version 2
$ErrorActionPreference = 'Stop'

function exec([string]$_cmd) {
    Write-Host ">>> $_cmd $args" -ForegroundColor Cyan
    $ErrorActionPreference = 'Continue'
    & $_cmd @args
    $ErrorActionPreference = 'Stop'
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Failed with exit code $LASTEXITCODE"
        exit 1
    }
}

Remove-Item -Recurse $Output -ErrorAction Ignore

exec dotnet run `
        -p "src/Resume" `
        -- validate

exec dotnet run `
        -p "src/Resume" `
        -- build -o "$Output/artifacts"

Copy-Item -Path "./resume.json" -Destination "$Output/artifacts"
Copy-Item -Path "./scripts/publish.ps1" -Destination $Output
