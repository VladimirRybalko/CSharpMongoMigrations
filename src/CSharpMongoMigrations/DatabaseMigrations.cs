using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace CSharpMongoMigrations
{
    /// <summary>
    /// Represent the existing database migrations and allow to apply new migrations.
    /// </summary>
    /// <remarks>For internal usage only.</remarks>
    internal interface IDatabaseMigrations
    {
        List<MigrationVersion> GetAppliedMigrations(string collection);

        void ApplyMigration(MigrationVersion version);
        void CancelMigration(MigrationVersion version);
        
        IMongoDatabase GetDatabase();
    }

    internal class DatabaseMigrations : IDatabaseMigrations
    {
        private readonly IMongoDatabase _db;
        private readonly string _collectionName = typeof(MigrationVersion).Name;

        public DatabaseMigrations(MongoUrl url)
        {
            var client = MongoClientFactory.Get(url);
            _db = client.GetDatabase(url.DatabaseName);
        }

        public List<MigrationVersion> GetAppliedMigrations(string collection)
        {
           return _db.GetCollection<MigrationVersion>(_collectionName)
                .Find(x => string.IsNullOrEmpty(collection) || x.Collection == collection)
                .SortByDescending(x => x.Version)
                .ToList();
        }

        public IMongoDatabase GetDatabase()
        {
            return _db;
        }

        public void ApplyMigration(MigrationVersion version)
        {
            _db.GetCollection<MigrationVersion>(_collectionName).InsertOne(version);
        }

        public void CancelMigration(MigrationVersion version)
        {
            _db.GetCollection<MigrationVersion>(_collectionName).DeleteOne(x => x.Version == version.Version && x.Collection == version.Collection);
        }
    }
}