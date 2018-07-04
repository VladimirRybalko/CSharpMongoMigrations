using System;
using MongoDB.Driver;
using Xunit;

namespace CSharpMongoMigrations.Tests
{
    public class MigrationRunnerTests
    {
        [Fact]
        public void ShouldNotCreateMigrationRunnerWithNullUrl()
        {
            Assert.Throws<NullReferenceException>(() =>
            {
                var runner = new MigrationRunner((MongoUrl)null, "test");
            });
        }

        [Fact]
        public void ShouldNotCreateMigrationRunnerWithInvalidConnectionString()
        {
            Assert.Throws<MongoConfigurationException>(() =>
            {
                var runner = new MigrationRunner("test", "test");
            });
        }
    }
}
