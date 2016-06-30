using CSharpMongoMigrations.Extensions;
using MongoDB.Driver;
using System.Linq;
using System;

namespace CSharpMongoMigrations
{
    public interface IDatabaseMigrations
    {
        MigrationVersion GetLastAppliedMigration();

        void ApplyMigration(MigrationVersion version);
        void CancelMigration(MigrationVersion version);


        IMongoDatabase GetDatabase();
    }

    public class DatabaseMigrations : IDatabaseMigrations
    {
        private readonly IMongoDatabase _db;
        private readonly string _collectionName = typeof(MigrationVersion).Name;

        public DatabaseMigrations(string server, string database)
        {
            var client = MongoClientFactory.Get($"mongodb://{server}");
            _db = client.GetDatabase(database);
        }

        public MigrationVersion GetLastAppliedMigration()
        {
            var lastMigration = _db.GetCollection<MigrationVersion>(_collectionName)
                .FindAll()
                .SortByDescending(x => x.Version)
                .ToList()
                .FirstOrDefault();

            return lastMigration ?? new MigrationVersion();
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
