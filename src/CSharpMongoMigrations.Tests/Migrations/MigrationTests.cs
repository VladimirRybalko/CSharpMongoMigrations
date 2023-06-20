//using CSharpMongoMigrations.Migrations;
using FluentAssertions;
using MongoDB.Driver;
using Moq;
using System.ComponentModel;
using Xunit;

namespace CSharpMongoMigrations.Tests
{
    public class MigrationFacts
    {
        private class MigrationStub : Migration
        {
            public IMongoDatabase Db { get { return Database; } }

            public override void Down()
            {
            }

            public override void Up()
            {
            }

            public override bool ShouldUp()
            {
                return false;
            }

            public override bool ShouldDown()
            {
                return false;
            }
        }


        [Fact]
        [Description("Check IDbMigration.UseDatabase method. It's used only for internal purpose.")]
        public void UseDatabaseFact()
        {
            // Arrange
            var migration = new MigrationStub();
            var db = new Mock<IMongoDatabase>();

            // Act
            ((IDbMigration)migration).UseDatabase(db.Object);

            // Assert
            migration.Db.Should().Be(db.Object);
        }

        [Fact]
        public void ConditionalMigrationFact()
        {
            // Arrange
            var migration = new MigrationStub();

            // Act
            var upResult = migration.ShouldUp();
            var downResult = migration.ShouldDown();

            // Assert
            upResult.Should().Be(false);
            downResult.Should().Be(true);
        }
    }
}
