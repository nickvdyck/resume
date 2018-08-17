<#
 .SYNOPSIS
    Deploys a blob to Azure

 .DESCRIPTION
    Deploys artifacts to an Azure Blob Storage Account

 .PARAMETER containerName
    The subscription id where the template will be deployed.

 .PARAMETER artifacts
    Folder to upload to azure
#>

param(
    [string]
    $containerName = "resume",

    [string]
    $artifacts = "./artifacts"
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

$container = az storage container show --name $containerName

if (!$container)
{
    Write-Host "Storage container does not exist creating container..."

    $result = (az storage container create --name $containerName | ConvertFrom-Json)

    if ($result.created -ne $true) {
        Write-Error "Something went wrong creating storage container"
    }

    $container = az storage container show --name $containerName
}

$files = Get-ChildItem -Path $artifacts

foreach ($file in $files)
{
    Write-Host "Uploading file $($file.FullName)"
    $success = $(
        az storage blob upload `
            --container-name $containerName `
            --name $file.Name `
            --file $file.FullName
    )

    if (!$success)
    {
        throw "Error while upload file: $file.FullName"
    }
}

Write-Host "Done..."
