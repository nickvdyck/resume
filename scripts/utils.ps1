function Exec([string] $_cmd) {
    Write-Host ">>> $_cmd $args" -ForegroundColor Cyan
    $ErrorActionPreference = 'Continue'
    & $_cmd @args
    $ErrorActionPreference = 'Stop'
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Failed with exit code $LASTEXITCODE"
        exit 1
    }
}
