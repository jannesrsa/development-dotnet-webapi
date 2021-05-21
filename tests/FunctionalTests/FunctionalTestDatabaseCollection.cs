using Xunit;

namespace FunctionalTests
{
    [CollectionDefinition(FunctionalTestDatabaseCollection.CollectionName, DisableParallelization = true)]
    public class FunctionalTestDatabaseCollection : ICollectionFixture<FunctionalTestDatabaseFixture>
    {
        public const string CollectionName = "FunctionalTestsCollectionName";

        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}