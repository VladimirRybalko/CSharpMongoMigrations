using CSharpMongoMigrations.Migrations;

namespace CSharpMongoMigrations
{
    internal class VersionedMigration : IMigration
    {
        private readonly IMigration _migration;
        private readonly IConditionalMigration _conditionalMigration;

        public MigrationVersion Version { get; private set; }

        public VersionedMigration(IMigration migration, MigrationVersion version,
            IConditionalMigration conditionalMigration)
        {
            _migration = migration;
            Version = version;
            _conditionalMigration = conditionalMigration;
        }

        public void Down()
        {
            _migration.Down();
        }

        public void Up()
        {
            _migration.Up();
        }

        public bool ShouldUp()
        {
            return _conditionalMigration.ShouldUp();
        }

        public bool ShouldDown()
        {
            return _conditionalMigration.ShouldDown();
        }
    }
}