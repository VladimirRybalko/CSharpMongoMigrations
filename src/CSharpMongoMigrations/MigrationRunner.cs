using System;
using System.Linq;

namespace CSharpMongoMigrations
{
    public class MigrationRunner
    {
        private readonly IDatabaseMigrations _dbMigrations;
        private readonly IMigrationLocator _locator;
        private readonly string _migrationAssembly;

        public MigrationRunner(string server, string database, string migrationAssembly)
        {
            _dbMigrations = new DatabaseMigrations(server, database);
            _locator = new MigrationLocator(migrationAssembly, _dbMigrations.GetDatabase());

            _migrationAssembly = migrationAssembly;
        }

        public void Up(long version = -1)
        {
            version = version == -1 ? long.MaxValue : version;

            Console.WriteLine($"Discovering migrations in {_migrationAssembly}");
            
            var lastMigrationVersion = _dbMigrations.GetLastAppliedMigration();
            var inapplicableMigrations = _locator.GetMigrations(lastMigrationVersion, new MigrationVersion(version)).ToList();

            Console.WriteLine($"Found ({inapplicableMigrations.Count}) migrations in {_migrationAssembly}");

            foreach (var migration in inapplicableMigrations)
            {
                Console.WriteLine($"Applying: {migration.Version}"); 
                            
                migration.Up();
                _dbMigrations.ApplyMigration(migration.Version);

                Console.WriteLine($"Applied: {migration.Version}");
            }
        }

        public void Down(long version = -1)
        {
            Console.WriteLine($"Discovering migrations in {_migrationAssembly}");

            var lastMigrationVersion = _dbMigrations.GetLastAppliedMigration();
            var downgradedMigrations = _locator.GetMigrations(new MigrationVersion(version), lastMigrationVersion).ToList();

            Console.WriteLine($"Found ({downgradedMigrations.Count}) migrations in {_migrationAssembly}");

            foreach (var migration in downgradedMigrations)
            {
                Console.WriteLine($"Applying: {migration.Version}");

                migration.Down();
                _dbMigrations.CancelMigration(migration.Version);

                Console.WriteLine($"Applied: {migration.Version}");
            }
        }
    }
}
