using System;
using MongoDB.Driver;
using Moq;
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
                var runner = new MigrationRunner((MongoUrl)null, "test", new MigrationFactory());
            });
        }

        [Fact]
        public void ShouldNotCreateMigrationRunnerWithInvalidConnectionString()
        {
            Assert.Throws<MongoConfigurationException>(() =>
            {
                var runner = new MigrationRunner("test", "test", new MigrationFactory());
            });
        }

        [Fact]
        public void ShouldNotCreateMigrationRunnerWithNullFactory()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var runner = new MigrationRunner("mongodb://valid", "test", factory: null);
            });
        }
    }
}
