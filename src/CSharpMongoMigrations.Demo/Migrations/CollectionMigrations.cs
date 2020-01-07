using MongoDB.Bson;
using MongoDB.Driver;
using System;

namespace CSharpMongoMigrations.Demo.Migrations
{
    [Migration("Animals", 0)]
    public sealed class AddAnimalMigration : Migration
    {
        public override void Up()
        {
            var collection = GetCollection("Animals");
            var document = new BsonDocument();

            document.AddUniqueIdentifier(new Guid("2A7B73A8-3C4A-422D-90B4-C73BCF48EBD4"));
            document.AddProperty("Kind", "Cat");

            collection.InsertOne(document);
        }

        public override void Down()
        {
            var collection = GetCollection("Animals");
            var idFilter = Builders<BsonDocument>.Filter.Eq("_id", new Guid("2A7B73A8-3C4A-422D-90B4-C73BCF48EBD4"));
            collection.DeleteOne(idFilter);
        }
    }

    [Migration("Animals", 1)]
    public sealed class ChangeAnimalKindMigration : DocumentMigration
    {
        protected override string CollectionName { get { return "Animals"; } }

        protected override void UpgradeDocument(BsonDocument document)
        {
            document.ChangeValue("Kind", "Dog");
        }

        protected override void DowngradeDocument(BsonDocument document)
        {
            document.ChangeValue("Kind", "Cat");
        }
    }
}