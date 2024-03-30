﻿#Launch WebApi Website and POST a request for generating client APIs
cd $PSScriptRoot
$path = "$PSScriptRoot\DemoCoreWeb\bin\Debug\net7.0"
$procArgs = @{
    FilePath         = "dotnet.exe"
    ArgumentList     = "$path\DemoCoreWeb.dll"
    WorkingDirectory = $path
    PassThru         = $true
}
$process = Start-Process @procArgs

Invoke-RestMethod http://localhost:5000/api/DateTypes/GetDateTimeOffset -Method GET
Invoke-RestMethod http://localhost:5000/api/DateTypes/GetDateOnly -Method GET

