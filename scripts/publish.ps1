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
    [string]
    $ContainerName = "resume",

    [string]
    $Source = "./artifacts"
)

#******************************************************************************
# Script body
# Execution begins here
#******************************************************************************
$ErrorActionPreference = "Stop"

if (!$env:AZURE_STORAGE_CONNECTION_STRING)
{
    throw "Azure connection string should be provided as an environment variable"
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

$result = (
    az storage blob upload-batch `
        --destination $ContainerName `
        --source $Source
)

Write-Host "Done..."
