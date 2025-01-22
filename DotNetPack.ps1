$packCmd = 'dotnet pack $Name --no-build --output C:/NugetLocalFeeds --configuration Release'
$projList = 'Fonlow.Testing.HttpCore/Fonlow.Testing.HttpCore.csproj', 'Fonlow.Testing.ServiceCore/Fonlow.Testing.ServiceCore.csproj', 'Fonlow.Testing.Basic/Fonlow.Testing.Basic.csproj', 'Fonlow.Testing.Integration/Fonlow.Testing.Integration.csproj', 'Fonlow.Testing.Utilities/Fonlow.Testing.Utilities.csproj'
foreach($name in $projList){
    Invoke-Expression $ExecutionContext.InvokeCommand.ExpandString($packCmd)
}