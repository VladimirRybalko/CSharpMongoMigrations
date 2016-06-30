using MongoDB.Driver;

namespace CSharpMongoMigrations
{
    internal interface IDbMigration : IMigration
    {
        void SetDatabase(IMongoDatabase database);
    }
}
