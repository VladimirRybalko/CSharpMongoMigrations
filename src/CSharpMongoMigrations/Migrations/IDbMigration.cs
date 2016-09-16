using MongoDB.Driver;

namespace CSharpMongoMigrations
{
    /// <summary>
    /// Represent mongo database migration
    /// </summary>
    internal interface IDbMigration : IMigration
    {
        /// <summary>
        /// Set a mongo database for migration
        /// </summary>
        /// <param name="database"></param>
        void UseDatabase(IMongoDatabase database);
    }
}
