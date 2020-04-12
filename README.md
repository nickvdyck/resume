# Resume

[![Build status][ci-badge]][ci-url]

## Introduction
This repository is the source of truth for my resume. It uses a schema defined by [JSON Resume](https://jsonresume.org/), which is an open-source initiative to create a JSON-based standard for resumes.

This project uses a [cli tool](https://github.com/nickvdyck/resume-cli) to validate and build my resume. The repository is hooked up to a CI pipeline which executes a build on every push to master. The pipeline validates, builds and publishes my resume to a blob storage where my assets are hosted.

## How to build

Download [.NET Core 3.1](https://dotnet.microsoft.com/download) or newer.
Once installed, you can use the following commands to generate a html and pdf version of my resume:

### Unix
```sh
make build
```

### Windows
```powershell
./scripts/build.ps1
```

[ci-url]: https://github.com/nickvdyck/resume
[ci-badge]: https://github.com/nickvdyck/resume/workflows/CI/badge.svg
