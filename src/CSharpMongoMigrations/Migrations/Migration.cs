using MongoDB.Bson;
using MongoDB.Driver;

namespace CSharpMongoMigrations
{
    public abstract class Migration : IDbMigration
    {
        protected IMongoDatabase Database { get; private set; }

        void IDbMigration.SetDatabase(IMongoDatabase database)
        {
            Database = database;
        }

        public virtual void Down()
        {
        }        

        public virtual void Up()
        {
        }

        protected IMongoCollection<BsonDocument> GetCollection(string name)
        {
            return Database.GetCollection<BsonDocument>(name);
        }       
    }
}
