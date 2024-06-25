# Fonlow.Testing.Utilities

Common utilities for setting up and tearing down assets used in integration testing, such as CopyItems.

## NuGet Packages

### For .NET Core 8.0 +

* Package [Fonlow.Testing.Integration](https://www.nuget.org/packages/Fonlow.Testing.Integration/), including all packages.
* Package [Fonlow.Testing.ServiceCore](https://www.nuget.org/packages/Fonlow.Testing.ServiceCore/)
	* Class [ServiceCommandsFixture](https://github.com/zijianhuang/FonlowTesting/blob/master/Fonlow.Testing.ServiceCore/ServiceCommandsFixture.cs)
* Package [Fonlow.Testing.HttpCore](https://www.nuget.org/packages/Fonlow.Testing.HttpCore/)
	* Class [BasicHttpClient](https://github.com/zijianhuang/FonlowTesting/blob/master/Fonlow.Testing.HttpCore/BasicHttpClient.cs)
	* Class [HttpClientWithUsername](https://github.com/zijianhuang/FonlowTesting/blob/master/Fonlow.Testing.HttpCore/HttpClientWithUsername.cs)
	* Class [TestingSettings](https://github.com/zijianhuang/FonlowTesting/blob/master/Fonlow.Testing.HttpCore/TestingSettings.cs)
* [Examples of Integration Test Suite](https://github.com/zijianhuang/DemoCoreWeb/tree/master/Tests/ServiceCommandIntegrationTests)

