using MongoDB.Bson;
using MongoDB.Driver;
using System;

namespace CSharpMongoMigrations.Demo.Migrations
{
    [Migration(4, "Add Migration when condition meets")]
    public sealed class ConditionalMigrations : Migration
    {
        public override void Up()
        {
            var collection = GetCollection("Persons");
            var document = new BsonDocument();

            document.AddUniqueIdentifier(new Guid("20C2CAC4-C55D-4C5C-8937-33698A3EC6C7"));
            document.AddProperty("Name", "John Doe - Conditional");

            collection.InsertOne(document);
        }

        public override void Down()
        {
            var collection = GetCollection("Persons");
            var idFilter = Builders<BsonDocument>.Filter.Eq("_id", new Guid("20C2CAC4-C55D-4C5C-8937-33698A3EC6C7"));
            collection.DeleteOne(idFilter);
        }

        // Check your condition
        public override bool ShouldUp()
        {
            return false;
        }
    }
}
