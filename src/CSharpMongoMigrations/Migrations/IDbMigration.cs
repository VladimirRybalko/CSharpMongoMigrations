using MongoDB.Driver;

namespace CSharpMongoMigrations
{
    /// <summary>
    /// Represent mongo database migration
    /// </summary>
    public interface IDbMigration : IMigration
    {
        /// <summary>
        /// Set database for migration
        /// </summary>
        /// <param name="database"></param>
        void SetDatabase(IMongoDatabase database);
    }
}
