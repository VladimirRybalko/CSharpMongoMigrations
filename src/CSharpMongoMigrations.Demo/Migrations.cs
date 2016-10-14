using MongoDB.Bson;
using MongoDB.Driver;
using System;

namespace CSharpMongoMigrations.Demo
{
    [Migration(0, "Add John Doe")]
    public class AddPersonMigration : Migration
    {
        public override void Up()
        {
            var collection = GetCollection("Persons");
            var document = new BsonDocument();

            document.AddUniqueIdentifier(new Guid("06BFFCF5-DAE9-422A-85AB-F58DE41E86DA"));
            document.AddProperty("Name", "John Doe");
            
            collection.InsertOne(document);
        }

        public override void Down()
        {
            var collection = GetCollection("Persons");
            var idFilter = Builders<BsonDocument>.Filter.Eq("_id", new Guid("06BFFCF5-DAE9-422A-85AB-F58DE41E86DA"));
            collection.DeleteOne(idFilter);
        }
    }


    [Migration(1, "Change persons")]
    public class AddPropertyPersonMigration : DocumentMigration
    {
        protected override string CollectionName { get { return "Persons"; } }
               
        protected override void UpgradeDocument(BsonDocument document)
        {
            document.AddProperty("IsActive", true);            
        }

        protected override void DowngradeDocument(BsonDocument document)
        {
            document.RemoveProperty("IsActive");
        }
    }


    [Migration(2, "Change property")]
    public class ChangePropertyMigration : DocumentMigration
    {
        protected override string CollectionName { get { return "Persons"; } }

        protected override void UpgradeDocument(BsonDocument document)
        {
            document.ChangeValue("IsActive", false);
        }

        protected override void DowngradeDocument(BsonDocument document)
        {
            document.ChangeValue("IsActive", true);
        }
    }

    [Migration(3, "Add type property")]
    public class AddTypePropertyMigration : DocumentMigration
    {
        protected override string CollectionName { get { return "Persons"; } }

        protected override void UpgradeDocument(BsonDocument document)
        {
            document.AddProperty("Type", 0);
        }

        protected override void DowngradeDocument(BsonDocument document)
        {
            document.RemoveProperty("Type");
        }
    }
}
