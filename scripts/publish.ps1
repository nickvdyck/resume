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

    [Parameter(Mandatory = $True)]
    [string]
    $StorageAccountName,

    [string]
    $ContainerName = "resume",

    [string]
    $Source = "$PSScriptRoot/../artifacts"

)

#******************************************************************************
# Script body
# Execution begins here
#******************************************************************************

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

. "$PSScriptRoot/utils.ps1"

$config = (
    az storage account show-connection-string `
        --name $StorageAccountName |
    ConvertFrom-Json
)

if (!$config) {
    throw "Azure connection string should be provided as an environment variable (AZURE_STORAGE_CONNECTION_STRING)"
} else {
    $env:AZURE_STORAGE_CONNECTION_STRING = $config.connectionString
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

$env:AZURE_STORAGE_CONNECTION_STRING = $null

Write-Host 'Done' -ForegroundColor Magenta
