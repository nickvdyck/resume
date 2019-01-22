# Resume

[![Build Status](https://dev.azure.com/vandycknick/resume/_apis/build/status/nickvdyck.resume)](https://dev.azure.com/vandycknick/resume/_build/latest?definitionId=1)

## Introduction
This repository is the source of truth for my resume. It uses a schema defined by [JSON Resume](https://jsonresume.org/), which is a open source initiative to create a JSON-based standard for resumes.

This project uses a cli tool to validate and build my resume. The repository is hooked up to a CI pipeline which executes a build on every push to master. The pipeline validates, builds and publishes my resume to a blob storage where my assets are hosted.

## How to build
The following script will publish a html, pdf and json version of my resume.

```powershell
./scripts/build.ps1
```

## Structure

```sh
├── scripts
│   ├── build.ps1
│   ├── publish.ps1
│   └── utils.ps1
├── src
│   └── resume.json
├── README.md
...
```

The scripts folder contains `build.ps1` and `publish.ps1`, these scripts can be used to build my resume and deploy the output to an azure blob storage container.

- `scripts/build.ps1`: Used in CI to validate and build my resume. This will an `html` and `pdf` artefact of my resume.
- `scripts/publish.ps1`: Will publish artefacts created by `build.ps1` script to a blob container. You will need to set an environment variable `AZURE_STORAGE_CONNECTION_STRING` with a connection string to you storage account.
- `scripts/utils.ps1`: some powershell utility functions
- `src/resume.json`: My actual resume in a json format defined by [JSON Resume](https://jsonresume.org/).
