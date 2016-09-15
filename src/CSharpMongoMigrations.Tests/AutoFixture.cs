using Ploeh.AutoFixture;

namespace CSharpMongoMigrations.Tests
{
    public static class AutoFixture
    {
        private static readonly Fixture _fixture = new Fixture();

        public static T Create<T>()
        {
            return _fixture.Create<T>();
        }
    }
}
