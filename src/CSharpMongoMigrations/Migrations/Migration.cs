using CSharpMongoMigrations.Migrations;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CSharpMongoMigrations
{
    /// <summary>
    /// Base class for mongo migrations
    /// </summary>
    public abstract class Migration : IDbMigration, IConditionalMigration
    {
        protected IMongoDatabase Database { get; private set; }

        void IDbMigration.UseDatabase(IMongoDatabase database)
        {
            Database = database;
        }
        
        public abstract void Up();
        public abstract void Down();
        
        protected IMongoCollection<BsonDocument> GetCollection(string name)
        {
            return Database.GetCollection<BsonDocument>(name);
        }

        public virtual bool ShouldUp() => true;
        public virtual bool ShouldDown() => true;
    }
}
