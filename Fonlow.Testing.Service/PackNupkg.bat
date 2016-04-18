cd %~dp0
nuget.exe pack Fonlow.Testing.Service.csproj -Prop Configuration=Release -OutputDirectory c:\release\FonlowTesting
pause