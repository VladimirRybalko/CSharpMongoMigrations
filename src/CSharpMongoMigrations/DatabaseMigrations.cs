using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace CSharpMongoMigrations
{
    /// <summary>
    /// Represent the existing database migrations and allow to apply new migrations 
    /// </summary>
    /// <remarks>For internal using only</remarks>
    internal interface IDatabaseMigrations
    {
        List<MigrationVersion> GetAppliedMigrations();

        void ApplyMigration(MigrationVersion version);
        void CancelMigration(MigrationVersion version);
        
        IMongoDatabase GetDatabase();
    }

    internal class DatabaseMigrations : IDatabaseMigrations
    {
        private readonly IMongoDatabase _db;
        private readonly string _collectionName = typeof(MigrationVersion).Name;

        public DatabaseMigrations(string server, string database)
        {
            var client = MongoClientFactory.Get($"mongodb://{server}");
            _db = client.GetDatabase(database);
        }

        public List<MigrationVersion> GetAppliedMigrations()
        {
           return _db.GetCollection<MigrationVersion>(_collectionName)
                .FindAll()
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
            _db.GetCollection<MigrationVersion>(_collectionName).DeleteOne(x => x.Version == version.Version);
        }
    }
}
