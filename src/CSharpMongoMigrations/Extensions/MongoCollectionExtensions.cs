using MongoDB.Bson;
using MongoDB.Driver;

namespace CSharpMongoMigrations
{
    /// <summary>
    /// Provides mongo collection extensions.
    /// </summary>
    public static class MongoCollectionExtensions
    {
        /// <summary>
        /// Update a document in the specified collection.
        /// </summary>
        /// <param name="collection">The target collection.</param>
        /// <param name="document">The desired document for updating.</param>
        public static void Update(this IMongoCollection<BsonDocument> collection, BsonDocument document)
        {
            var idFilter = Builders<BsonDocument>.Filter.Eq("_id", document.GetValue("_id"));            
            collection.ReplaceOne(idFilter, document);
        }

        /// <summary>
        /// Find all documents in the specified collection.
        /// </summary>
        /// <typeparam name="T">Document type.</typeparam>
        /// <param name="collection">The target collection.</param>
        /// <returns></returns>
        public static IFindFluent<T, T> FindAll<T>(this IMongoCollection<T> collection)
        {
            return collection.Find(new BsonDocument());
        }
    }
}