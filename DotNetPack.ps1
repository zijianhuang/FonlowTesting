$packCmd = 'dotnet pack $Name --no-build --output C:/NugetLocalFeeds --configuration Release'
$projList = 'Fonlow.Testing.HttpCore/Fonlow.Testing.HttpCore.csproj', 'Fonlow.Testing.ServiceCore/Fonlow.Testing.ServiceCore.csproj'
foreach($name in $projList){
    Invoke-Expression $ExecutionContext.InvokeCommand.ExpandString($packCmd)
}