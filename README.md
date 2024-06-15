# FonlowTesting

For the sake of CI, TDD, BDD, unit testing and integration testing, is it a CI server mandatory like TFS, TeamCity, Bamboo or Azure DevOps etc.? 

Sometimes it could be handy and costing less to setup CI environment in each dev machine. Developers endorsing XP or TDD have been doing so for years before those off-the-shelf CI products were released to the market for team CI/CD.

A typical integration [test suite](https://en.wikipedia.org/wiki/Test_suite) should have the dependencies ready automatically before running. While a CI server generally has some built-in mechanism to launch respective dependencies and then run those test suites, it will be nicer that the integration test suite can take care of the dependencies at some degree, especially for those in-house service applications.

This project is to deliver some light-weight helper classes for developers to quickly construct integration tests on their own dev PC and share across dev team members. Even if your team is using a team CI/CD server, the helper classes may still help carrying out a lot integration tests before reaching the CI server. Also, if a test suite can take good care of setting up dependencies and tearing down them, the scripts/configuration on the team CI/CD server could be simplified, and the locked-in effect on a particular brand of team CI/CD product could be the least.

**Remarks:**
* A dedicated CI server generally provides comprehensive and powerful mechanisms of setting up and tearing down dependencies, like GitHub Actions/Workflows. This library will remain light-weight and serve as a complementary tool for overall CI.

## NuGet Packages

### For .NET Core 8.0 +

* [Fonlow.Testing.ServiceCore](https://www.nuget.org/packages/Fonlow.Testing.ServiceCore/)
* [Fonlow.Testing.HttpCore](https://www.nuget.org/packages/Fonlow.Testing.HttpCore/)
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

        "Username": "admin",
        "Password": "Pppppp*8"
    }
}
```

Integration Test Suite:

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

## Recommended/Expected Practices

The design of these helper classes are based on the author's understanding about software engineering. If you share the following understandings, you may find the helper classes could be useful giving you smoother developer experience during DevOps or developing cloud-based applications.

For the sake of DevOps, ensure the application codes have zero or the least knowledge of the IT operations.

For the sake of Cloud computing or so called cloud native, ensure the application codes or the primary logics have zero or the least knowledge of the host applications and the cloud environment, after ensuring the overall architectural design and software design have the least coupling with the cloud environment.

When doing CI configuration, prefer configuration convention over fine-grained configuration. For example, build the sln, then run all tests, rather than craft fine-grained controls over each library and application of the same sln.

There are definitely some legitimate cases that make the above recommendations sound illegitimate .

# Alternatives

The libraries of helper classes have been developed since the .NET Framework era. These days since .NET Core, there have been more well designed libraries around:

* [Functional Testing ASP.NET Core Apps](https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/test-asp-net-core-mvc-apps#functional-testing-aspnet-core-apps)
* [Docker Compose Fixture](https://github.com/devjoes/DockerComposeFixture)
* [Integration Test with .Net Core and Docker](https://ademcatamak.medium.com/integration-test-with-net-core-and-docker-21b241f7372)