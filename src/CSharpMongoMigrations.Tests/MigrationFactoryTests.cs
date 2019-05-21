using FluentAssertions;
using MongoDB.Driver;
using Xunit;

namespace CSharpMongoMigrations.Tests
{
    public class MigrationFactoryTests
    {
        private class MigrationStub : Migration
        {
            public IMongoDatabase Db
            {
                get { return Database; }
            }

            public override void Down()
            {
            }

            public override void Up()
            {
            }
        }

        [Fact]
        public void UseDatabaseFact()
        {
            // Arrange
            var factory = new MigrationFactory();

            // Act
            var migration = factory.Create(typeof(MigrationStub));

            // Assert
            migration.Should().NotBe(null);
            migration.Should().BeOfType<MigrationStub>();
        }
    }
}
