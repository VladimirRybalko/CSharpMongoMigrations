using System.Collections.Concurrent;
using MongoDB.Driver;

namespace CSharpMongoMigrations
{
    /// <summary>
    /// The helper factory to store a connection to the mongo database.
    /// Preventing creation of multiple connections for the same mongo database.
    /// </summary>
    internal class MongoClientFactory
    {
        private static readonly ConcurrentDictionary<string, IMongoClient> _clients = new ConcurrentDictionary<string, IMongoClient>();

        /// <summary>
        /// Get mongo client by Mongo connection URL
        /// </summary>
        /// <param name="mongoUrl">Mongo connection Url</param>
        /// <returns></returns> 
        public static IMongoClient Get(MongoUrl mongoUrl)
        { 
            var mongoUrlStr = mongoUrl.ToString();
            if (!_clients.ContainsKey(mongoUrlStr))
                _clients.TryAdd(mongoUrlStr, new MongoClient(mongoUrl));

            return _clients[mongoUrlStr];
        }
    }
}