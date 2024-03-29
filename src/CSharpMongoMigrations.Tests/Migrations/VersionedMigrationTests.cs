﻿using FluentAssertions;
using Moq;
using Xunit;

namespace CSharpMongoMigrations.Tests
{
    public class VersionedMigrationFacts
    {
        [Fact]
        public void CtsFact()
        {
            // Arrange
            var migration = new Mock<IMigration>();
            var version = AutoFixture.Long();

            // Act
            var versionedMigration = new VersionedMigration(migration.Object, new MigrationVersion(version));

            // Assert
            versionedMigration.Version.Version.Should().Be(version);
        }

        [Fact]
        public void UpFact()
        {
            // Arrange
            var migration = new Mock<IMigration>();
            migration.Setup(x => x.Up());
            var versionedMigration = new VersionedMigration(migration.Object, MigrationVersion.Max(null));

            // Act
            versionedMigration.Up();

            // Assert
            migration.VerifyAll();
        }

        [Fact]
        public void DownFact()
        {
            // Arrange
            var migration = new Mock<IMigration>();
            migration.Setup(x => x.Down());
            var version = AutoFixture.Long();
            var versionedMigration = new VersionedMigration(migration.Object, MigrationVersion.Min(null));

            // Act
            versionedMigration.Down();

            // Assert
            migration.VerifyAll();
        }

        [Fact]
        public void ShouldUpFact()
        {
            // Arrange
            var migration = new Mock<IMigration>();
            migration.Setup(x => x.ShouldUp());
            var versionedMigration = new VersionedMigration(migration.Object, MigrationVersion.Min(null));

            // Act
            versionedMigration.ShouldUp();

            // Assert
            migration.VerifyAll();
        }

        [Fact]
        public void ShouldDownFact()
        {
            // Arrange
            var migration = new Mock<IMigration>();
            migration.Setup(x => x.ShouldDown());
            var versionedMigration = new VersionedMigration(migration.Object, MigrationVersion.Min(null));

            // Act
            versionedMigration.ShouldDown();

            // Assert
            migration.VerifyAll();
        }
    }
}