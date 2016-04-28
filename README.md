# FonlowTesting

For the sack of CI, TDD, BDD, unit testing and integration testing, is it a CI server mandatary like TFS, TeamCity or Bamboo etc.? 

If your project is not having over 1 million code statements involving more than 5 developers working around the clock, the overhead of setting up or using dedicated CI environments could be quite substantial. Sometimes, for small and medium projects, combining a unit testing framework like MS Test / NUnit / xUnit, along with some batch scripts, Powershell scripts as well as some light-weight helper classes could still compose a pretty health Continuous Integration at fairly low cost.

This project is to deliver some light-weight helper classes for developers to quickly constructing integration tests by individual developers on their own dev PC. Even if your team is using a CI server, the helpr classes may still help carrying out a lot integration tests before reaching the CI server.

For more details, please read wiki at https://github.com/zijianhuang/FonlowTesting/wiki

