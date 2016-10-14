using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace CSharpMongoMigrations.Tests.Migrations
{
    [TestFixture(TestOf = typeof(VersionedMigration))]
    public class VersionedMigrationTests
    {
        [Test]
        public void CtsTest()
        {
            // Arrange
            var migration = new Mock<IMigration>();
            var version = AutoFixture.Create<long>();
            
            // Act
            var versionedMigration = new VersionedMigration(migration.Object, new MigrationVersion(version));

            // Assert
            versionedMigration.Version.Version.Should().Be(version);
        }

        [Test]
        public void UpTest()
        {
            // Arrange
            var migration = new Mock<IMigration>();
            migration.Setup(x => x.Up());
            var versionedMigration = new VersionedMigration(migration.Object, MigrationVersion.Max);

            // Act
            versionedMigration.Up();

            // Assert
            migration.VerifyAll();
        }

        [Test]
        public void DownTest()
        {
            // Arrange
            var migration = new Mock<IMigration>();
            migration.Setup(x => x.Down());
            var version = AutoFixture.Create<long>();
            var versionedMigration = new VersionedMigration(migration.Object, MigrationVersion.Min);

            // Act
            versionedMigration.Down();

            // Assert
            migration.VerifyAll();
        }
    }
}
