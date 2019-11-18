# FonlowTesting

For the sake of CI, TDD, BDD, unit testing and integration testing, is it a CI server mandatary like TFS, TeamCity or Bamboo etc.? 

If your project is not having over 1 million code statements involving more than 5 developers working around the clock, the overhead of setting up or using off-the-shelf CI environments could be quite substantial comparing to benefits. Sometimes, for small and medium projects, combining a unit testing framework like MS Test / NUnit / xUnit, along with some batch scripts, Powershell scripts as well as some light-weight helper classes could still compose a pretty healthy Continuous Integration at fairly low cost. And being able to construct and execute integration tests in a local development machine will encourage more frequent integration tests, and should make integration tests in an off-the-shelf CI server, and avoid embarrassing situation that have a CI server, but have no continuous integration but only continuous build.

This project is to deliver some light-weight helper classes for developers to quickly constructing integration tests by individual developers on their own dev PC. Even if your team is using a CI server, the helper classes may still help carrying out a lot integration tests before reaching the CI server.

For more details, please read wiki at https://github.com/zijianhuang/FonlowTesting/wiki



## NuGet Packages

### For .NET Framework 4.5.2 +

* [Fonlow.Testing.Service](https://www.nuget.org/packages/Fonlow.Testing.Service/)
* [Fonlow.Testing.Http](https://www.nuget.org/packages/Fonlow.Testing.Http/)
* [Examples](https://github.com/zijianhuang/webapiclientgenexamples/tree/master/Tests/IntegrationTests)


### For .NET Core 3.0 +

* [Fonlow.Testing.ServiceCore](https://www.nuget.org/packages/Fonlow.Testing.ServiceCore/)
* [Fonlow.Testing.HttpCore](https://www.nuget.org/packages/Fonlow.Testing.HttpCore/)
* [Examples](https://github.com/zijianhuang/DemoCoreWeb/tree/master/Tests/IntegrationTestsCore)