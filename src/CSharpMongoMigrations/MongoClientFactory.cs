using MongoDB.Driver;
using System.Collections.Generic;

namespace CSharpMongoMigrations
{
    public class MongoClientFactory
    {
        private static readonly Dictionary<string, IMongoClient> _clients = new Dictionary<string, IMongoClient>();

        public static IMongoClient Get(string server)
        {
            if (!_clients.ContainsKey(server))
                _clients.Add(server, new MongoClient(server));

            return _clients[server];
        }
    }
}
