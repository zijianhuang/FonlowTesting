using Xunit;

namespace IntegrationTests
{
    /// <summary>
    /// 
    /// </summary>
    [CollectionDefinition("ServiceLaunch")]
    public class DotNetHostCollection : ICollectionFixture<Fonlow.Testing.DotNetHostFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
