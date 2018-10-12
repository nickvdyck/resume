# Resume

[![Build Status](https://dev.azure.com/vandycknick/resume/_apis/build/status/nickvdyck.resume)](https://dev.azure.com/vandycknick/resume/_build/latest?definitionId=1)

## Introduction
This repository holds the data for my resume. It uses a schema defined by [JSON Resume](https://jsonresume.org/), which is a open source initiative to create a JSON-based standard for resumes.

This project uses a cli tool to validate and build my resume. The repository is hooked up to azure pipelines to execute and build on every commit and than to push those changes in a continues integrated fashion to my azure blob storage where my assets are hosted.

## Getting started

Download the [2.1.402](https://www.microsoft.com/net/download) .NET Core SDK or newer.
This should install all the required tools and binaries required to build this project.

## Structure

```sh
├── scripts
├── src
│   ├── Resume
│   ├── Resume.Models
│   └── Resume.Views
└── resume.json
```

The scripts contains `build.ps1` and `publish.ps1`, these scripts are just to respectively build my project and deploy the output to azure blob storage.

- `src/Resume`: Is a cli tool that can be used to build and/or validate your `resume.json` file.
- `src/Resume.Models`: Contains resume domain models.
- `src/Resume.Views`: Contains razor views to build an html page or pdf from a `resume.json` file. These views are consumed by the resume cli tool.
- `resume.json`: My actual resume in a json format defined by [JSON Resume](https://jsonresume.org/).
