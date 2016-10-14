using System;
using System.Linq;

namespace CSharpMongoMigrations
{
    /// <summary>
    /// Migration runner. Responsible for running migrations from specified assembly
    /// </summary>
    /// <remarks></remarks>
    public class MigrationRunner
    {
        private readonly IDatabaseMigrations _dbMigrations;
        private readonly IMigrationLocator _locator;
        private readonly string _migrationAssembly;

        /// <summary>
        /// Cts
        /// </summary>
        /// <param name="server">MongoDb server connection string</param>
        /// <param name="database">Mongo database name</param>
        /// <param name="migrationAssembly">Assembly with migrations</param>
        public MigrationRunner(string server, string database, string migrationAssembly)
        {
            _dbMigrations = new DatabaseMigrations(server, database);
            _locator = new MigrationLocator(migrationAssembly, _dbMigrations.GetDatabase());

            _migrationAssembly = migrationAssembly;
        }

        /// <summary>
        /// Apply all migrations up before specified version.
        /// Use -1 to apply all existing migrations
        /// </summary>
        /// <param name="version"></param>
        public void Up(long version = -1)
        {
            version = version == -1 ? long.MaxValue : version;

            Console.WriteLine($"Discovering migrations in {_migrationAssembly}");
            
            var appliedMigrations = _dbMigrations.GetAppliedMigrations();
            var inapplicableMigrations = 
                _locator.GetMigrations(MigrationVersion.Min, new MigrationVersion(version))
                .Where(m => appliedMigrations.All(x => x.Version != m.Version.Version))
                .ToList();

            Console.WriteLine($"Found ({inapplicableMigrations.Count}) migrations in {_migrationAssembly}");

            foreach (var migration in inapplicableMigrations)
            {
                Console.WriteLine($"Applying: {migration.Version}"); 
                            
                migration.Up();
                _dbMigrations.ApplyMigration(migration.Version);

                Console.WriteLine($"Applied: {migration.Version}");
            }
        }

        /// <summary>
        /// Apply all migrations down after specified version.
        /// Use -1 to downgrade all existing migrations
        /// </summary>
        /// <param name="version"></param>
        public void Down(long version = -1)
        {
            Console.WriteLine($"Discovering migrations in {_migrationAssembly}");

            var appliedMigrations = _dbMigrations.GetAppliedMigrations();
            var downgradedMigrations = 
                _locator.GetMigrations(new MigrationVersion(version), MigrationVersion.Max)
                .Where(m => appliedMigrations.Any(x => x.Version == m.Version.Version))
                .ToList();

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
