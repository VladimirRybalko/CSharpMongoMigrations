using System;
using System.Linq;
using System.Reflection;
using MongoDB.Driver;

namespace CSharpMongoMigrations
{
    /// <summary>
    /// The migration runner, that's responsible for running migrations from the user-specified assembly.
    /// </summary>
    public sealed class MigrationRunner
    {
        private readonly IDatabaseMigrations _dbMigrations;
        private readonly IMigrationLocator _locator;

        /// <summary>
        /// Creates a new instance of <seealso cref="MigrationRunner" />
        /// </summary>
        /// <param name="url">A MongoDb connection string.</param>
        /// <param name="migrationAssemblyName">The target assembly containing migrations.</param>
        /// <param name="factory">The factory that's responsible for instantiating migrations.</param>
        public MigrationRunner(MongoUrl url, string migrationAssemblyName, IMigrationFactory factory)
        {
            _dbMigrations = new DatabaseMigrations(url);
            _locator = new MigrationLocator(migrationAssemblyName, _dbMigrations.GetDatabase(), factory);
        }

        /// <summary>
        /// Creates a new instance of <seealso cref="MigrationRunner" />
        /// </summary>
        /// <param name="url">A MongoDb connection string.</param>
        /// <param name="migrationAssembly">The target assembly containing migrations.</param>
        /// <param name="factory">The factory that's responsible for instantiating migrations.</param>
        public MigrationRunner(MongoUrl url, Assembly migrationAssembly, IMigrationFactory factory)
        {
            _dbMigrations = new DatabaseMigrations(url);
            _locator = new MigrationLocator(migrationAssembly, _dbMigrations.GetDatabase(), factory);
        }


        /// <summary>
        /// Creates a new instance of  <seealso cref="MigrationRunner" />
        /// </summary>
        /// <param name="url">A MongoDb connection string.</param>
        /// <param name="migrationAssemblyName">The target assembly containing migrations.</param>
        public MigrationRunner(MongoUrl url, string migrationAssemblyName) :
            this(url, migrationAssemblyName, new MigrationFactory())
        {
        }

        /// <summary>
        /// Creates a new instance of <seealso cref="MigrationRunner" />
        /// </summary>
        /// <param name="connectionString">The Mongo connection string in common URL format.</param>
        /// <param name="migrationAssemblyName">The target assembly containing migrations.</param>
        /// <param name="factory">The factory that's responsible for instantiating migrations.</param>
        public MigrationRunner(string connectionString, string migrationAssemblyName, IMigrationFactory factory) :
            this(MongoUrl.Create(connectionString), migrationAssemblyName, factory)
        {
        }

        /// <summary>
        /// Creates a new instance of <seealso cref="MigrationRunner" />
        /// </summary>
        /// <param name="connectionString">The Mongo connection string in common URL format.</param>
        /// <param name="migrationAssemblyName">The target assembly containing migrations.</param>
        public MigrationRunner(string connectionString, string migrationAssemblyName) :
            this(MongoUrl.Create(connectionString), migrationAssemblyName, new MigrationFactory())
        {
        }

        /// <summary>
        /// Creates a new instance of <seealso cref="MigrationRunner" />
        /// </summary>
        /// <param name="connectionString">The Mongo connection string in common URL format.</param>
        /// <param name="migrationAssembly">The target assembly containing migrations.</param>
        public MigrationRunner(string connectionString, Assembly migrationAssembly) :
            this(MongoUrl.Create(connectionString), migrationAssembly, new MigrationFactory())
        {
        }

        /// <summary>
        /// Creates a new instance of <seealso cref="MigrationRunner" />
        /// </summary>
        /// <param name="server">The MongoDb server.</param>
        /// <param name="database">The MongoDb database name.</param>
        /// <param name="migrationAssembly">The target assembly containing migrations.</param>
        /// <param name="factory">The factory that's responsible for instantiating migrations.</param>
        public MigrationRunner(string server, string database, string migrationAssembly, IMigrationFactory factory) :
            this(MongoUrl.Create($"mongodb://{server}/{database}"), migrationAssembly, factory)
        {
        }

        /// <summary>
        /// Creates a new instance of <seealso cref="MigrationRunner" />
        /// </summary>
        /// <param name="server">The MongoDb server.</param>
        /// <param name="database">The MongoDb database name.</param>
        /// <param name="migrationAssembly">The target assembly containing migrations.</param>
        public MigrationRunner(string server, string database, string migrationAssembly) :
            this(MongoUrl.Create($"mongodb://{server}/{database}"), migrationAssembly, new MigrationFactory())
        {
        }

        /// <summary>
        /// Apply all migrations before specified version.
        /// Use -1 as a version parameter to apply all existing migrations.
        /// </summary>
        /// <param name="version">The desired migration version.</param>
        public void Up(long version = -1)
        {
            Up(null, version);
        }

        /// <summary>
        /// Apply all migrations before specified version for target collection.
        /// Use -1 as a version parameter to apply all existing migrations.
        /// </summary>
        /// <param name="collection">The target collection name.</param>
        /// <param name="version">The desired migration version.</param>
        public void Up(string collection, long version = -1)
        {
            version = version == -1 ? long.MaxValue : version;

            Console.WriteLine($"Discovering migrations in {_locator.LocatedAssembly.FullName}");

            var appliedMigrations = _dbMigrations.GetAppliedMigrations(collection);
            var inapplicableMigrations =
                _locator.GetMigrations(MigrationVersion.Min(collection), new MigrationVersion(collection, version))
                    .Where(m => appliedMigrations.All(x => x.Version != m.Version.Version || !string.Equals(x.Collection, m.Version.Collection)))
                    .OrderBy(x => x.Version.Collection)
                    .ThenBy(x => x.Version.Version)
                    .ToList();

            Console.WriteLine($"Found ({inapplicableMigrations.Count}) migrations in {_locator.LocatedAssembly.FullName}");

            foreach (var migration in inapplicableMigrations)
            {
                Console.WriteLine($"Applying: {migration.Version}");

                migration.Up();
                _dbMigrations.ApplyMigration(migration.Version);

                Console.WriteLine($"Applied: {migration.Version}");
            }
        }

        /// <summary>
        /// Roll back all migrations after specified version.
        /// Use -1 as a version parameter to downgrade all existing migrations.
        /// </summary>
        /// <param name="version">The desired migration version.</param>
        public void Down(long version = -1)
        {
            Down(null, version);
        }

        /// <summary>
        /// Roll back all collection migrations after specified version.
        /// Use -1 as a version parameter to downgrade all existing migrations.
        /// </summary>
        /// <param name="collection">The target collection name.</param>
        /// <param name="version">The desired migration version.</param>
        public void Down(string collection, long version = -1)
        {
            Console.WriteLine($"Discovering migrations in {_locator.LocatedAssembly.FullName}");

            var appliedMigrations = _dbMigrations.GetAppliedMigrations(collection);
            var downgradedMigrations =
                _locator.GetMigrations(new MigrationVersion(collection, version), MigrationVersion.Max(collection))
                    .Where(m => appliedMigrations.Any(x => x.Version == m.Version.Version && string.Equals(x.Collection, m.Version.Collection)))
                    .OrderByDescending(x => x.Version.Collection)
                    .ThenByDescending(x => x.Version.Version)
                    .ToList();

            Console.WriteLine($"Found ({downgradedMigrations.Count}) migrations in {_locator.LocatedAssembly.FullName}");

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