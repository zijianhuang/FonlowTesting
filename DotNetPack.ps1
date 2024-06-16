$packCmd = 'dotnet pack $Name --no-build --output ./Release --configuration Release'
$projList = 'Fonlow.Testing.HttpCore/Fonlow.Testing.HttpCore.csproj', 'Fonlow.Testing.ServiceCore/Fonlow.Testing.ServiceCore.csproj'
foreach($name in $projList){
    Invoke-Expression $ExecutionContext.InvokeCommand.ExpandString($packCmd)
}