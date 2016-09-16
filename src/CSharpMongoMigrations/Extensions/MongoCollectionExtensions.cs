using MongoDB.Bson;
using MongoDB.Driver;

namespace CSharpMongoMigrations
{
    /// <summary>
    /// Mongo collection extensions
    /// </summary>
    public static class MongoCollectionExtensions
    {
        /// <summary>
        /// Update document in specified collection
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="document">Document for updating</param>
        public static void Update(this IMongoCollection<BsonDocument> collection, BsonDocument document)
        {
            var idFilter = Builders<BsonDocument>.Filter.Eq("_id", document.GetValue("_id").AsGuid);            
            collection.ReplaceOne(idFilter, document);
        }

        /// <summary>
        /// Find all document in specified collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IFindFluent<T, T> FindAll<T>(this IMongoCollection<T> collection)
        {
            return collection.Find(new BsonDocument());
        }
    }
}
