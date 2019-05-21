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
        /// Discovery all migrations in specified assembly between specified versions
        /// </summary>
        /// <param name="after">The lower limit of the search</param>
        /// <param name="before">The upper limit of the search</param>
        /// <returns></returns>
        IEnumerable<VersionedMigration> GetMigrations(MigrationVersion after, MigrationVersion before);
    }

    internal class MigrationLocator : IMigrationLocator
    {
        private readonly Assembly _assembly;
        private readonly IMongoDatabase _database;
        private readonly IMigrationFactory _factory;

        public MigrationLocator(string assemblyName, IMongoDatabase database, IMigrationFactory factory)
        {
            _assembly = Assembly.Load(new AssemblyName(assemblyName));
            _database = database;
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        public IEnumerable<VersionedMigration> GetMigrations(MigrationVersion after, MigrationVersion before)
        {
            var migrations =
            (
                from type in _assembly.GetTypes()
                where typeof(IMigration).GetTypeInfo().IsAssignableFrom(type) && !type.GetTypeInfo().IsAbstract
                let attribute = type.GetTypeInfo().GetCustomAttribute<MigrationAttribute>()
                where attribute != null && after.Version < attribute.Version && attribute.Version <= before.Version
                orderby attribute.Version
                select new { Migration = _factory.Create(type), Version = new MigrationVersion(attribute.Version, attribute.Description) }
            ).ToList();

            foreach (var m in migrations)
                ((IDbMigration)m.Migration).UseDatabase(_database);

            return migrations.Select(x => new VersionedMigration(x.Migration, x.Version));
        }
    }
}
