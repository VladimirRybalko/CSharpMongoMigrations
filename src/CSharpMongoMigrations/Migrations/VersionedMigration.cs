﻿namespace CSharpMongoMigrations
{
    internal class VersionedMigration : IMigration
    {
        private readonly IMigration _migration;        

        public MigrationVersion Version { get; private set; }

        public VersionedMigration(IMigration migration, MigrationVersion version)
        {
            _migration = migration;
            Version = version;            
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
            return _migration.ShouldUp();
        }

        public bool ShouldDown()
        {
            return _migration.ShouldDown();
        }
    }
}