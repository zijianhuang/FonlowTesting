:: pack existing release build
set packCmd=dotnet pack --no-build --output C:\NugetLocalFeeds --configuration Release
%packCmd% Fonlow.Testing.HttpCore\Fonlow.Testing.HttpCore.csproj
%packCmd% Fonlow.Testing.ServiceCore\Fonlow.Testing.ServiceCore.csproj