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

### For .NET Framework 4.8 (Discontinued)

* [Fonlow.Testing.Service](https://www.nuget.org/packages/Fonlow.Testing.Service/)
* [Fonlow.Testing.Http](https://www.nuget.org/packages/Fonlow.Testing.Http/)
* [Examples](https://github.com/zijianhuang/webapiclientgenexamples/tree/master/Tests/IntegrationTests)

Tag LastNFSupport marks the last snapshot with .NET Framework supports. If you want to continue the support for .NET Framework, you may fork from that tag which includes libraries and integration test suites.