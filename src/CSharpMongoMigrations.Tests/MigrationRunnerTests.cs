using System;
using MongoDB.Driver;
using Xunit;

namespace CSharpMongoMigrations.Tests
{
    public class MigrationRunnerTests
    {
        [Theory]
        [InlineData("mongodb://test:27017/db", "System")]
        public void ShouldCreateMigrationRunnerWithMongoUrl(string connectionString, string assembly)
        {
            var url = MongoUrl.Create(connectionString);
            var runner = new MigrationRunner(url, assembly);
        }

        [Theory]
        [InlineData("mongodb://test:27017/db", "System")]
        public void ShouldCreateMigrationRunnerWithConnectionString(string connectionString, string assembly)
        {
            var runner = new MigrationRunner(connectionString, assembly);
        }

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
