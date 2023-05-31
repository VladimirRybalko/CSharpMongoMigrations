using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CSharpMongoMigrations
{
    /// <summary>
    /// Helper class for migrations discovery.
    /// </summary>
    internal interface IMigrationLocator
    {
        /// <summary>
        /// Discovery all migrations in the specified assembly in the current version's range.
        /// </summary>
        /// <param name="after">The lower limit of the search.</param>
        /// <param name="before">The upper limit of the search.</param>
        /// <returns></returns>
        IEnumerable<VersionedMigration> GetMigrations(MigrationVersion after, MigrationVersion before);

        Assembly LocatedAssembly { get; }
    }

    internal class MigrationLocator : IMigrationLocator
    {
        private readonly IMongoDatabase _database;
        private readonly IMigrationFactory _factory;

        public MigrationLocator(string assemblyName, IMongoDatabase database, IMigrationFactory factory)
        : this(Assembly.Load(new AssemblyName(assemblyName)), database, factory)
        {
        }

        public MigrationLocator(Assembly locatedAssembly, IMongoDatabase database, IMigrationFactory factory)
        {
            LocatedAssembly = locatedAssembly;
            _database = database;
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }
        public Assembly LocatedAssembly { get; }

        public IEnumerable<VersionedMigration> GetMigrations(MigrationVersion after, MigrationVersion before)
        {
            if (!string.Equals(after.Collection, before.Collection))
                throw new ArgumentException("Cannot apply cross collections migrations");

            var migrations =
            (
                from type in LocatedAssembly.GetTypes()
                where typeof(IMigration).GetTypeInfo().IsAssignableFrom(type) && !type.GetTypeInfo().IsAbstract
                let attribute = type.GetTypeInfo().GetCustomAttribute<MigrationAttribute>()
                where attribute != null 
                    && (string.IsNullOrEmpty(after.Collection) || string.Equals(attribute.Collection, after.Collection))
                    && after.Version < attribute.Version && attribute.Version <= before.Version
                select new { Migration = _factory.Create(type), Version = new MigrationVersion(attribute.Collection, attribute.Version, attribute.Description) }
            ).Where(migration => migration.Migration != null).ToList();

            foreach (var m in migrations)
                ((IDbMigration)m.Migration).UseDatabase(_database);

            return migrations.Select(x => new VersionedMigration(x.Migration, x.Version));
        }
    }
}