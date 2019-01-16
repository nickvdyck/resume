<#
 .SYNOPSIS
    Deploys a blob to Azure

 .DESCRIPTION
    Deploys artifacts to an Azure Blob Storage Account

 .PARAMETER ContainerName
    Azure blob storage container name

 .PARAMETER Source
    Folder to upload to azure blob storage container
#>

param(

    [String]
    $ContainerName = "resume",

    [String]
    $Source = "$PSScriptRoot/../artifacts"

)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

. "$PSScriptRoot/utils.ps1"

if ([String]::IsNullOrWhiteSpace($env:AZURE_STORAGE_CONNECTION_STRING)) {
    throw "Azure connection string should be provided as an environment variable (AZURE_STORAGE_CONNECTION_STRING)"
}

$container = az storage container show --name $ContainerName

if (!$container)
{
    Write-Host "Storage container does not exist creating container..."

    $result = (az storage container create --name $ContainerName | ConvertFrom-Json)

    if ($result.created -ne $true) {
        Write-Error "Something went wrong creating storage container"
    }

    $container = az storage container show --name $ContainerName
}

Exec az storage blob upload-batch `
        --destination $ContainerName `
        --source $Source

Write-Host 'Done' -ForegroundColor Magenta
