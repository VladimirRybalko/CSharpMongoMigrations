using FluentAssertions;
using MongoDB.Driver;
using Moq;
using NUnit.Framework;

namespace CSharpMongoMigrations.Tests.Migrations
{
    [TestFixture(TestOf = typeof(Migration))]
    public class MigrationTests
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
        }


        [Test]
        [Description("Check IDbMigration.UseDatabase method. It's used only for internal purpose.")]
        public void UseDatabaseTest()
        {
            // Arrange
            var migration = new MigrationStub();
            var db = new Mock<IMongoDatabase>();

            // Act
            ((IDbMigration)migration).UseDatabase(db.Object);

            // Assert
            migration.Db.Should().Be(db.Object);
        }
    }
}
