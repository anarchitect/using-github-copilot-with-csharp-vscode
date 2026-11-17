#Requires -Version 7.0
[CmdletBinding()]
param(
    [ValidateRange(1024, 65535)]
    [int]$BackEndPort = 5100,
    [ValidateRange(1024, 65535)]
    [int]$FrontEndPort = 5200,
    [switch]$NoBuild
)

$ErrorActionPreference = 'Stop'
$root = Split-Path $PSScriptRoot -Parent
$processes = [System.Collections.Generic.List[System.Diagnostics.Process]]::new()
$logs = Join-Path $root 'TestResults\smoke'
$api = "http://localhost:$BackEndPort"
$web = "http://localhost:$FrontEndPort"

function Assert-Demo([bool]$Condition, [string]$Message) {
    if (-not $Condition) {
        throw $Message
    }
    Write-Output "PASS: $Message"
}

function Wait-ForApp([System.Diagnostics.Process]$Process, [string]$Url) {
    $deadline = [DateTime]::UtcNow.AddSeconds(30)
    do {
        if ($Process.HasExited) {
            throw "App exited before it was ready. Read the logs in $logs."
        }
        try {
            $response = Invoke-WebRequest $Url -TimeoutSec 2 -SkipHttpErrorCheck
            if ($response.StatusCode -eq 200) { return }
        }
        catch [System.Net.Http.HttpRequestException] {
            # Connection refusal is expected while Kestrel starts.
        }
        catch [System.Threading.Tasks.TaskCanceledException] {
            # Retry a startup timeout until the overall deadline.
        }
        Start-Sleep -Milliseconds 250
    } while ([DateTime]::UtcNow -lt $deadline)
    throw "App did not become ready at $Url. Read the logs in $logs."
}

function Start-DemoApp([string]$Name, [string]$Url, [string[]]$ExtraArguments = @()) {
    $project = Join-Path $root "src\$Name\$Name.csproj"
    $arguments = @('run', '--project', "`"$project`"", '--no-build', '--no-launch-profile',
        '--', '--urls', $Url, '--environment', 'Development') + $ExtraArguments
    $process = Start-Process (Get-Command dotnet -ErrorAction Stop).Source `
        -ArgumentList $arguments -WorkingDirectory $root -PassThru `
        -RedirectStandardOutput (Join-Path $logs "$Name.out.log") `
        -RedirectStandardError (Join-Path $logs "$Name.err.log")
    $processes.Add($process)
    return $process
}

Push-Location $root
try {
    Assert-Demo ($BackEndPort -ne $FrontEndPort) 'API and web app use different ports'
    $usedPorts = [System.Net.NetworkInformation.IPGlobalProperties]::GetIPGlobalProperties().
        GetActiveTcpListeners().Port
    foreach ($port in @($BackEndPort, $FrontEndPort)) {
        if ($port -in $usedPorts) {
            throw "Port $port is already in use. Stop your demo apps or select different ports. No existing process was stopped."
        }
    }

    if (-not $NoBuild) {
        dotnet build WeatherApp.slnx --nologo
        if ($LASTEXITCODE -ne 0) { throw 'Build failed; no demo apps were started.' }
    }

    New-Item -ItemType Directory -Path $logs -Force | Out-Null
    $backEnd = Start-DemoApp 'BackEnd' $api
    Wait-ForApp $backEnd "$api/weatherforecast"
    $frontEnd = Start-DemoApp 'FrontEnd' $web @('--BackEndUrl', $api)
    Wait-ForApp $frontEnd $web

    $forecasts = Invoke-RestMethod "$api/weatherforecast"
    Assert-Demo ($forecasts.Count -eq 15) 'API returns 15 synthetic forecasts'
    $cities = Invoke-RestMethod "$api/weatherforecast/cities"
    Assert-Demo (($cities | Sort-Object) -join ',' -eq 'Brisbane,Melbourne,Sydney') 'API lists the three sample cities'
    foreach ($city in $cities) {
        $cityForecasts = Invoke-RestMethod "$api/weatherforecast/$city"
        Assert-Demo ($cityForecasts.Count -eq 5) "$city has five forecasts"
    }
    $unknown = Invoke-WebRequest "$api/weatherforecast/Atlantis" -SkipHttpErrorCheck
    Assert-Demo ($unknown.StatusCode -eq 404) 'An unknown city returns 404'
    $openApi = Invoke-RestMethod "$api/openapi/v1.json"
    Assert-Demo ($null -ne $openApi.paths.'/weatherforecast/{city}'.get) 'OpenAPI describes the city endpoint'
    $scalar = Invoke-WebRequest "$api/scalar"
    Assert-Demo ($scalar.Content -match 'scalar\.aspnetcore\.js') 'Scalar serves its API reference page'
    $scalarScript = Invoke-WebRequest "$api/scalar/scalar.js"
    Assert-Demo ($scalarScript.StatusCode -eq 200) 'Scalar serves its browser script'
    $weather = Invoke-WebRequest "$web/weather"
    Assert-Demo ([regex]::Matches($weather.Content, '<tr>').Count -eq 16) 'Blazor renders 15 weather rows and one header row'
    $css = Invoke-WebRequest "$web/app.css"
    Assert-Demo (($css.Headers.'Content-Type' -join ';') -match 'text/css') 'The web app serves its stylesheet'
    Write-Output "Smoke check passed. App logs: $logs"
    Write-Output 'This checks HTTP and server-rendered HTML, not browser interactivity or Copilot UI.'
}
finally {
    foreach ($process in $processes) {
        if (-not $process.HasExited) {
            $process.Kill($true)
            $process.WaitForExit()
        }
        $process.Dispose()
    }
    Pop-Location
}
