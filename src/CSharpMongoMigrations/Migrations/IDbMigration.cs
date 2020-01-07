using MongoDB.Driver;

using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("CSharpMongoMigrations.Tests")]

namespace CSharpMongoMigrations
{
    /// <summary>
    /// Represent a mongo database migration
    /// </summary>
    internal interface IDbMigration : IMigration
    {
        /// <summary>
        /// Set a mongo database to the current migration
        /// </summary>
        /// <param name="database"></param>
        void UseDatabase(IMongoDatabase database);
    }
}