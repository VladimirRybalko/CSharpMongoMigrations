using MongoDB.Driver;
using System.Collections.Generic;

namespace CSharpMongoMigrations
{
    /// <summary>
    /// Helper factory for storing connection to mongo database
    /// Preventing creation of multiple connections for the same mongo database 
    /// </summary>
    internal class MongoClientFactory
    {
        private static readonly Dictionary<string, IMongoClient> _clients = new Dictionary<string, IMongoClient>();

        /// <summary>
        /// Get mongo client by server connection string
        /// </summary>
        /// <param name="server">Mongo connection string</param>
        /// <returns></returns>
        public static IMongoClient Get(string server)
        {
            if (!_clients.ContainsKey(server))
                _clients.Add(server, new MongoClient(server));

            return _clients[server];
        }
    }
}
