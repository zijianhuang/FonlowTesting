# FonlowTesting

For the sake of CI, TDD, BDD, unit testing and integration testing, is it a CI server mandatary like TFS, TeamCity, or Bamboo etc.? 

Sometimes it could be handly and costing less to setup CI environment in each dev machine. Developers endorsing XP or TDD have been doing so for years before those off-the-shelf CI products were released to the market.

This project is to deliver some light-weight helper classes for developers to quickly constructing integration tests by individual developers on their own dev PC. Even if your team is using a CI server, the helper classes may still help carrying out a lot integration tests before reaching the CI server.

For more details, please read wiki at https://github.com/zijianhuang/FonlowTesting/wiki

## NuGet Packages

### For .NET Core 7.0 +

* [Fonlow.Testing.ServiceCore](https://www.nuget.org/packages/Fonlow.Testing.ServiceCore/)
* [Fonlow.Testing.HttpCore](https://www.nuget.org/packages/Fonlow.Testing.HttpCore/)
* [Examples](https://github.com/zijianhuang/DemoCoreWeb/tree/master/Tests/IntegrationTestsCore)

# Alternatives

The libraries of helper classes have been developed since the .NET Framework era. These days since .NET Core, there have been more well designed libraries around:

* [Functional Testing ASP.NET Core Apps](https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/test-asp-net-core-mvc-apps#functional-testing-aspnet-core-apps)
* [Docker Compose Fixture](https://github.com/devjoes/DockerComposeFixture)
* [Integration Test with .Net Core and Docker](https://ademcatamak.medium.com/integration-test-with-net-core-and-docker-21b241f7372)