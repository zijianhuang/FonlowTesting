$projList = 'Fonlow.Testing.HttpCore/Fonlow.Testing.HttpCore.csproj', 'Fonlow.Testing.ServiceCore/Fonlow.Testing.ServiceCore.csproj', 'Fonlow.Testing.Basic/Fonlow.Testing.Basic.csproj', 'Fonlow.Testing.Integration/Fonlow.Testing.Integration.csproj', 'Fonlow.Testing.Utilities/Fonlow.Testing.Utilities.csproj'
foreach($name in $projList){
    $packCmd = 'dotnet pack $name --no-build --output C:/NugetLocalFeeds --configuration Release'
    Invoke-Expression $ExecutionContext.InvokeCommand.ExpandString($packCmd)
}