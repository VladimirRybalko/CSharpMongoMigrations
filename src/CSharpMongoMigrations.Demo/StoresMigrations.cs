using System;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CSharpMongoMigrations.Demo
{
    [Migration(1, "Create Index", "Stores")]
    public class Createindex : Migration
    {
        public override void Up()
        {
            var collection = GetCollection("Stores");
            var index = new CreateIndexModel<BsonDocument>("{ location: '2dsphere' }", new CreateIndexOptions
            {
                Background = true,
                Name = "location_2dsphere"
            });

            collection.Indexes.CreateOne(index);
        }

        public override void Down()
        {
            var collection = GetCollection("Stores");

            collection.Indexes.DropOne("location_2dsphere");
        }
    }
}