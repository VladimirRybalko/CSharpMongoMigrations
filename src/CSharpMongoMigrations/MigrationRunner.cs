using System;
using System.Linq;
using MongoDB.Driver;

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
        private readonly string _collectionName;
        private readonly string _migrationAssembly;

        /// <summary>
        /// Creates a new instance of MigrationRunner
        /// </summary>
        /// <param name="url">MongoDb connection string</param>
        /// <param name="collectionName">MongoDb collection name</param>
        /// <param name="migrationAssembly">Assembly with migrations</param>
        public MigrationRunner(MongoUrl url, string collectionName, string migrationAssembly)
        {
            _dbMigrations = new DatabaseMigrations(url);
            _collectionName = collectionName;
            _locator = new MigrationLocator(migrationAssembly, _dbMigrations.GetDatabase());

            _migrationAssembly = migrationAssembly;
        }

        /// <summary>
        /// Creates a new instance of MigrationRunner
        /// </summary>
        /// <param name="connectionString">connection string in common URL format</param>
        /// <param name="collectionName">MongoDb collection name</param>
        /// <param name="migrationAssembly">Assembly containing migrations</param>
        public MigrationRunner(string connectionString, string collectionName, string migrationAssembly) : 
            this(MongoUrl.Create(connectionString), collectionName, migrationAssembly)
        {
        }

        /// <summary>
        /// Creates a new instance of MigrationRunner
        /// </summary>
        /// <param name="server">MongoDb server</param>
        /// <param name="database">MongoDb database</param>
        /// <param name="collectionName">MongoDb collection name</param>
        /// <param name="migrationAssembly">Assembly containing migrations</param>
        public MigrationRunner(string server, string database, string collectionName, string migrationAssembly) :
            this(BuildConnectionString(server, database), collectionName, migrationAssembly)
        {
        }

        /// <summary>
        /// Apply all migrations before specified version.
        /// Use -1 to apply all existing migrations
        /// </summary>
        /// <param name="version"></param>
        public void Up(long version = -1)
        {
            version = version == -1 ? long.MaxValue : version;

            Console.WriteLine($"Discovering migrations in {_migrationAssembly}");
            
            var appliedMigrations = _dbMigrations.GetAppliedMigrations(this._collectionName);
            var inapplicableMigrations = 
                _locator.GetMigrations(MigrationVersion.Min, new MigrationVersion(version), this._collectionName)
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
        /// Rool back all migrations after specified version.
        /// Use -1 to downgrade all existing migrations
        /// </summary>
        /// <param name="version"></param>
        public void Down(long version = -1)
        {
            Console.WriteLine($"Discovering migrations in {_migrationAssembly}");

            var appliedMigrations = _dbMigrations.GetAppliedMigrations(this._collectionName);
            var downgradedMigrations = 
                _locator.GetMigrations(new MigrationVersion(version), MigrationVersion.Max, this._collectionName)
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

        private static string BuildConnectionString(string server, string database)
        {
            var serverPart = server.StartsWith("mongodb://") ? server : $"mongodb://{server}";

            return $"{serverPart}/{database}";
        }
    }
}
