# FonlowTesting

For the sake of CI, TDD, BDD, unit testing and integration testing, is it a CI server mandatory like TFS, TeamCity, Bamboo or Azure DevOps etc.? 

Sometimes it could be handy and costing less to setup CI environment in each dev machine. Developers endorsing XP or TDD have been doing so for years before those off-the-shelf CI products were released to the market for team CI/CD.

A typical integration [test suite](https://en.wikipedia.org/wiki/Test_suite) should have the dependencies ready automatically before running. While a CI server generally has some built-in mechanism to launch respective dependencies and then run those test suites, it will be nicer that the integration test suite can take care of the dependencies at some degree, especially for those in-house service applications.

This project is to deliver some light-weight helper classes for developers to quickly construct integration tests on their own dev PC and share across dev team members. Even if your team is using a team CI/CD server, the helper classes may still help carrying out a lot integration tests before reaching the CI server. Also, if a test suite can take good care of setting up dependencies and tearing down them, the scripts/configuration on the team CI/CD server could be simplified, and the locked-in effect on a particular brand of team CI/CD product could be the least.

**Remarks:**
* A dedicated CI server generally provides comprehensive and powerful mechanisms of setting up and tearing down dependencies, like GitHub Actions/Workflows. This library will remain light-weight and serve as a complementary tool for overall CI.

## NuGet Packages

### For .NET Core 8.0 +

* Package [Fonlow.Testing.ServiceCore](https://www.nuget.org/packages/Fonlow.Testing.ServiceCore/)
	* Class [ServiceCommandsFixture](https://github.com/zijianhuang/FonlowTesting/blob/master/Fonlow.Testing.ServiceCore/ServiceCommandsFixture.cs)
* Package [Fonlow.Testing.HttpCore](https://www.nuget.org/packages/Fonlow.Testing.HttpCore/)
	* Class [BasicHttpClient](https://github.com/zijianhuang/FonlowTesting/blob/master/Fonlow.Testing.HttpCore/BasicHttpClient.cs)
	* Class [HttpClientWithUsername](https://github.com/zijianhuang/FonlowTesting/blob/master/Fonlow.Testing.HttpCore/HttpClientWithUsername.cs)
	* Class [TestingSettings](https://github.com/zijianhuang/FonlowTesting/blob/master/Fonlow.Testing.HttpCore/TestingSettings.cs)
* [Examples of Integration Test Suite](https://github.com/zijianhuang/DemoCoreWeb/tree/master/Tests/ServiceCommandIntegrationTests)

## Examples

appsettings.json:
```json
{
	"Testing": {
		"ServiceCommands": [
			{
				"CommandPath": "../../../../../PoemsMyDbCreator/bin/{BuildConfiguration}/net8.0/PoemsMyDbCreator.exe",
				"Arguments": "Fonlow.EntityFrameworkCore.MySql \"server=localhost;port=3306;Uid=root; password=zzzzzzzz; database=Poems_Test; Persist Security Info=True;Allow User Variables=true\"",
				"Delay": 0
			},

			{
				"CommandPath": "dotnet",
				"Arguments": "run --project ../../../../../PoetryApp/PoetryApp.csproj --no-build --configuration {BuildConfiguration}",
				"BaseUrl": "http://localhost:5300/",
				"Delay": 1
			}
		],
...
...
	}
}
```

The setting will instruct the ServiceCommandsFixture as a collection fixture to use PoemsMyDbCreator.exe that will tear-down the DB and create a new one, then launch Web API PoetryApp that uses the blank new DB.

**Remarks**

* You may have an integration test suite that test the Data Access Layer based on Entity Framework (Core) or the Business Logic Layer using something similar to "PoemsMyDbCreator".

Integration Test Suite which is a client app to localhost:5300:

```csharp
	public class TestConstants
	{
		public const string LaunchWebApiAndInit = "LaunchWebApi";
	}

	[CollectionDefinition(TestConstants.LaunchWebApiAndInit)]
	public class DotNetHostCollection : ICollectionFixture<Fonlow.Testing.ServiceCommandsFixture>
	{
	}

	public class PoemsFixture : AuthHttpClientWithUsername
	{
		public PoemsFixture()
		{
			System.Text.Json.JsonSerializerOptions jsonSerializerSettings = new System.Text.Json.JsonSerializerOptions()
			{
				DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
				PropertyNameCaseInsensitive = true,
			};

			Api = new PoemsApp.Controllers.Client.Poems(AuthorizedClient, jsonSerializerSettings);
			TagsApi = new PoemsApp.Controllers.Client.Tags(AuthorizedClient, jsonSerializerSettings);
		}

		public PoemsApp.Controllers.Client.Poems Api { get; private set; }
		public PoemsApp.Controllers.Client.Tags TagsApi { get; private set; }
	}

	[Collection(TestConstants.LaunchWebApiAndInit)]
	public class PoemsTests : IClassFixture<PoemsFixture>
	{
		public PoemsTests(PoemsFixture fixture)
		{
			api = fixture.Api;
			tagsApi = fixture.TagsApi;
			authorizedClient = fixture.AuthorizedClient;
		}

		readonly PoemsApp.Controllers.Client.Poems api;
		readonly PoemsApp.Controllers.Client.Tags tagsApi;
		readonly HttpClient authorizedClient;

		[Fact]
		public async Task TestAddPoemAndUpdatePublished()
		{
			var p = await api.AddAsync(new Poem
			{
```

"ServiceCommandsFixture" will replace {BuildConfiguration} with the build configuration of the test suite.

Alternative, you may have "appsettings.Debug.json", "appsettings.Release.json" or even something like "appsettings.MacRelease.json" together with "appsettings.json".

appsettings.Debug.json:
```json
{
	"Testing": {
		"ServiceCommands": [
			{
				"CommandPath": "dotnet",
				"Arguments": "run --project ../../../../../DemoCoreWeb/DemoCoreWeb.csproj --no-build --configuration Debug",
				"BaseUrl": "http://127.0.0.1:5000/",
				"Delay": 2
			}
		]
	}
}
```

## Settings

```csharp
public sealed class TestingSettings
{
	public string Username { get; set; }
	public string Password { get; set; }

	/// <summary>
	/// For testing with many different user credentials.
	/// </summary>
	public UsernamePassword[] Users { get; set; }

	public ServiceCommand[] ServiceCommands { get; set; }

	/// <summary>
	/// Build configuration such as Debug, Release or whatever custom build configuration. 
	/// ServiceCommandFixture will replace {BuildConfiguration} in arguments with this.
	/// </summary>
	public string BuildConfiguration { get; private set; }
}

public sealed class UsernamePassword
{
	public string Username { get; set; }
	public string Password { get; set; }
}

public sealed class ServiceCommand
{
	public string CommandPath { get; set; }
	public string Arguments { get; set; }

	/// <summary>
	/// Some services may take some seconds to launch then listen, especially in GitHub Actions which VM/container could be slow. A good bet may be 5 seconds.
	/// </summary>
	public int Delay { get; set; }
	public string ConnectionString { get; set; }
	public string BaseUrl { get; set; }
}
```

## More Examples of Launching Services/Commands

### In-house Web API

```json
{
	"CommandPath": "dotnet",
	"Arguments": "run --project ../../../../../DemoCoreWeb/DemoCoreWeb.csproj --no-build --configuration {BuildConfiguration}",
}
```
Hints:

* The current directory of the launched Web API is the directory containing the csproj file.

```json
{
	"CommandPath": "dotnet",
	"Arguments": "../../../../../DemoCoreWeb/bin/{BuildConfiguration}/net8.0/DemoCoreWeb.dll",
}
```

Hints:

* The current directory of the launched Web API is the directory of the test suite. Thus if some features of Web API depends on the locations of current directory, content root path and Web root path, such launch may result in problems, for example, it cannot find some files in some default relative locations.

```json
{
	"CommandPath": "../../../../../DemoCoreWeb/bin/{BuildConfiguration}/net8.0/DemoCoreWeb.exe",
}
```

Hints:

* The current directory of the launched Web API is the directory of the EXE file. And this is recommended.
* The only setback comparing with launching through the csproj file is that after upgrading to next .NET version, you need to adjust the .NET version in the path, for example, from "net8.0" to "net9.0" and so on.

## GitHub Workflow

For .NET developers, GitHub actions provides a "dotnet.yml" which by default run the debug build. Sometimes it may be more appropriate to run release build. Therefore, change the last 2 steps, like:
```yml
    - name: Build
      run: dotnet build --no-restore --configuration Release
    - name: Test
      run: dotnet test --no-build --verbosity normal --configuration Release
```
And name the file as "dotnetRelease.yml".

If the integration test suites depend on relatively simple "out-of-process" resources such as ASP.NET Core Web services, the helper classes of "Fonlow.Testing.ServiceCore" could be simple and handy enough before you need to craft more complex GitHub workflows.

Examples:

* https://github.com/zijianhuang/webapiclientgen/actions
* https://github.com/zijianhuang/openapiclientgen/actions

# Alternatives

The libraries of helper classes have been developed since the .NET Framework era. These days since .NET Core, there have been more well designed libraries around:

* [Functional Testing ASP.NET Core Apps](https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/test-asp-net-core-mvc-apps#functional-testing-aspnet-core-apps)
* [Docker Compose Fixture](https://github.com/devjoes/DockerComposeFixture)
* [Integration Test with .Net Core and Docker](https://ademcatamak.medium.com/integration-test-with-net-core-and-docker-21b241f7372)
* [BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet)

**References:**

* [Different Types of Tests](https://www.atlassian.com/continuous-delivery/software-testing/types-of-software-testing)