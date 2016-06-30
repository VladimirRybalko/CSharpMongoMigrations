using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;

namespace CSharpMongoMigrations.Extensions
{
    public static class MongoCollectionExtensions
    {
        public static void Update(this IMongoCollection<BsonDocument> collection, BsonDocument document)
        {
            var idFilter = Builders<BsonDocument>.Filter.Eq("_id", document.GetValue("_id").AsGuid);
            collection.ReplaceOne(idFilter, document);
        }

        public static IFindFluent<T, T> FindAll<T>(this IMongoCollection<T> collection)
        {
            return collection.Find(new BsonDocument());
        }
    }
}
