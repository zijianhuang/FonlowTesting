#Launch WebApi Website and POST a request for generating client APIs
$path = "$PSScriptRoot/DemoCoreWeb/bin/Debug/net8.0"
$procArgs = @{
    FilePath         = "dotnet"
    ArgumentList     = "$path/DemoCoreWeb.dll"
    WorkingDirectory = $path
    PassThru         = $true
}
$process = Start-Process @procArgs

Invoke-RestMethod http://localhost:5000/WeatherForecast -Method GET