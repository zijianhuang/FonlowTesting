#Launch WebApi Website and POST a request for generating client APIs
cd $PSScriptRoot
$path = "$PSScriptRoot/DemoCoreWeb/bin/Debug/net8.0"
$procArgs = @{
    FilePath         = "dotnet.exe"
    ArgumentList     = "$path/DemoCoreWeb.dll"
    WorkingDirectory = $path
    PassThru         = $true
}
$process = Start-Process @procArgs

Invoke-RestMethod http://127.0.0.1:5000/WeatherForecast -Method GET